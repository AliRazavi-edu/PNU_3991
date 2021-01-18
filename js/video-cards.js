const video = document.getElementById("video");
const sidePanelEl = document.getElementById("side_panel");

let currentTime = 0;

/**
 * Parse cards and create card objects and bind them to the video.
 * @param {Object[]} cards 
 */
function createCards(cards) {
    parseCardTimestamps(cards);

    bindVideoListener(cards);
}

/**
 * Parse the cards and process their timestamps into seconds values from a 
 * given modules object array.
 * @param {Object[]} cards 
 */
function parseCardTimestamps(cards) {
    // calculate the seconds value of the timestamp
    cards
        .forEach(card => {
            if (card.seconds) return;

            // parses timestamps HH:MM:SS
            const seconds = card.timestamp
                .split(":")
                .map((x, i, arr) => {
                    const weight = arr.length - i;

                    // hours
                    if (weight == 3) 
                        return parseInt(x, 10) * 60 * 60;
                    // minutes
                    else if (weight == 2)
                        return parseInt(x, 10) * 60;
                    // seconds
                    else
                        return parseInt(x, 10);
                })
                .reduce((a, b) => a + b);

            card.seconds = seconds;
        });
}

/**
 * Bind the video listener function to the video element which handles what components to show depending 
 * on video time.
 */
function bindVideoListener(cards) {
    if (!video) return;

    video.ontimeupdate = e => {
        currentTime = Math.floor(e.srcElement.currentTime);

        // check if new modules need to be displayed
        cards
            .filter(x => x.seconds == currentTime)
            .forEach(card => {
                // if a module is already passed, don't do anything
                if (card.passed) 
                {
                    return;
                }
                else
                {
                    // lockdown player to force user to acknowledge/interact with the module
                    video.pause();
                    lockPlayer();
                }

                // if module is not already rendered
                if (!card.mounted)
                {
                    // create the card element and append it to the side panel element
                    sidePanelEl.appendChild(createComponent(card));
                    card.mounted = true;

                    // definitions are to be seen, but require no interaction to pass them
                    if (card.type == "definition")
                    {
                        card.passed = true;

                        unlockPlayer();
                    }
                }
            });
        
        // TODO save modules status to localStorage?
    }
}

/**
 * Convert a module definition object into HTML that can be displayed.
 * @param {Object} card 
 */
function createComponent(card) {
    // create card element that will render the module data
    const cardEl = document.createElement("div");
    cardEl.className = "card slide-in mb-1";

    // create a header element that displays the card type and the dismissal button
    const cardHeaderEl = document.createElement("h5");
    cardHeaderEl.classList.add("card-header");
    cardHeaderEl.innerText = createTitle(card.type);

    // create the dismissal button and add it to the header element
    const dismissalButtonEl = document.createElement("button");
    dismissalButtonEl.classList.add("close");
    dismissalButtonEl.innerHTML = "&times;";
    dismissalButtonEl.onclick = () => {
        // only allow a user to dismiss the card if they have passed it
        if (card.passed) 
        {
            cardEl.classList.add("fade-out");

            setTimeout(() => sidePanelEl.removeChild(cardEl), 300);

            video.play();
        }
    }
    cardHeaderEl.appendChild(dismissalButtonEl);

    const data = card.data;
    let title, body;

    const cardBodyEl = document.createElement("div");
    cardBodyEl.classList.add("card-body");

    if (card.type == "definition")
    {
        title = data.term;

        body = data.definition;

        cardBodyEl.innerHTML = `
            <h5 class="card-title mb-1">
                ${title}
            </h5>

            <p class="card-text">
                ${body}
            </p>
        `;
    }
    else if (card.type == "multiple_choice")
    {
        title = data.question;

        cardBodyEl.innerHTML += `
            <h5 class="card-title mb-1">
                ${title}
            </h5>
        `;

        // shuffle the answer choices for variety sake and create the radio buttons for them
        body = shuffle(data.options)
            .forEach((option, i) => {
                const radioButton = document.createElement("div");
                radioButton.classList.add("form-check");
                radioButton.innerHTML = `
                    <input class="form-check-input" name="answers" type="radio" id="check${i}" value="${option}"/>

                    <label class="form-check-label" for="check${i}">
                        ${option}
                    </label>
                `;
                
                cardBodyEl.appendChild(radioButton);
            });

        cardBodyEl.innerHTML += `<p class="feedback mb-1 mt-1" style="display: none !important"></p>`;

        // create a check answer button that can unlock the player
        const checkAnswerButtonEl = document.createElement("button");
        checkAnswerButtonEl.className = "btn btn-success mt-1";
        checkAnswerButtonEl.innerText = "Check Answer";
        checkAnswerButtonEl.onclick = () => {
            let userAnswer = cardBodyEl.querySelector("input[name='answers']:checked").value;

            const feedbackEl = cardBodyEl.querySelector("p.feedback");
            feedbackEl.style.display = "";

            if (userAnswer == data.answer)
            {
                card.passed = true;

                feedbackEl.style.color = "green";
                feedbackEl.innerText = "That's correct!";

                unlockPlayer();
            }
            else
            {
                feedbackEl.style.color = "red";
                feedbackEl.innerText = "That's incorrect.";
            }
        }

        cardBodyEl.appendChild(checkAnswerButtonEl);
    }

    cardEl.appendChild(cardHeaderEl);
    cardEl.appendChild(cardBodyEl);

    return cardEl;
}

/**
 * Get a card type and convert it to title case.
 * @param {String} type 
 * @returns {String} title
 */
function createTitle(type) {
    return type
        .split("_")
        .map(x => x.substring(0, 1).toUpperCase() + x.substring(1))
        .join(" ");
}

/**
 * Prevent the user from playing the video and seeking.
 */
function lockPlayer() {
    video.classList.add("no-play");
    video.onplay = () => video.pause();
    video.onseeking = e => video.currentTime = currentTime; // TODO allow seeking to times before frozen spot?
}

/**
 * Allow the user to use the player as normal.
 */
function unlockPlayer() {
    video.classList.remove("no-play");
    video.onplay = () => {};
    video.onseeking = () => {};
}

/**
 * Shuffles the array that is given.
 * @param {any[]} array 
 * @return {any[]} shuffledArray
 */
function shuffle(array) {
    for (let i = array.length - 1; i > 0; i--) {
        const j = Math.floor(Math.random() * (i + 1));
        [array[i], array[j]] = [array[j], array[i]];
    }

    return array;
}
