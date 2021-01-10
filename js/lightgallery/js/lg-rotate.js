/**!
 * lg-rotate.js | 1.2.0-beta.0 | October 5th 2020
 * http://sachinchoolur.github.io/lg-rotate.js
 * Copyright (c) 2016 Sachin N; 
 * @license GPLv3 
 */(function(f){if(typeof exports==="object"&&typeof module!=="undefined"){module.exports=f()}else if(typeof define==="function"&&define.amd){define([],f)}else{var g;if(typeof window!=="undefined"){g=window}else if(typeof global!=="undefined"){g=global}else if(typeof self!=="undefined"){g=self}else{g=this}g.LgRotate = f()}})(function(){var define,module,exports;return (function(){function r(e,n,t){function o(i,f){if(!n[i]){if(!e[i]){var c="function"==typeof require&&require;if(!f&&c)return c(i,!0);if(u)return u(i,!0);var a=new Error("Cannot find module '"+i+"'");throw a.code="MODULE_NOT_FOUND",a}var p=n[i]={exports:{}};e[i][0].call(p.exports,function(r){var n=e[i][1][r];return o(n||r)},p,p.exports,r,e,n,t)}return n[i].exports}for(var u="function"==typeof require&&require,i=0;i<t.length;i++)o(t[i]);return o}return r})()({1:[function(require,module,exports){
(function (global, factory) {
    if (typeof define === "function" && define.amd) {
        define([], factory);
    } else if (typeof exports !== "undefined") {
        factory();
    } else {
        var mod = {
            exports: {}
        };
        factory();
        global.lgRotate = mod.exports;
    }
})(this, function () {
    'use strict';

    var _extends = Object.assign || function (target) {
        for (var i = 1; i < arguments.length; i++) {
            var source = arguments[i];

            for (var key in source) {
                if (Object.prototype.hasOwnProperty.call(source, key)) {
                    target[key] = source[key];
                }
            }
        }

        return target;
    };

    var rotateDefaults = {
        rotate: true,
        rotateLeft: true,
        rotateRight: true,
        flipHorizontal: true,
        flipVertical: true
    };

    var Rotate = function Rotate(element) {

        this.core = window.lgData[element.getAttribute('lg-uid')];
        this.core.s = _extends({}, rotateDefaults, this.core.s);

        if (this.core.s.rotate && this.core.doCss()) {
            this.init();
        }

        return this;
    };

    Rotate.prototype.buildTemplates = function () {
        var rotateIcons = '';
        if (this.core.s.flipVertical) {
            rotateIcons += '<button aria-label="flip vertical" class="lg-flip-ver lg-icon"></button>';
        }
        if (this.core.s.flipHorizontal) {
            rotateIcons += '<button aria-label="Flip horizontal" class="lg-flip-hor lg-icon"></button>';
        }
        if (this.core.s.rotateLeft) {
            rotateIcons += '<button aria-label="Rotate left" class="lg-rotate-left lg-icon"></button>';
        }
        if (this.core.s.rotateRight) {
            rotateIcons += '<button aria-label="Rotate right" class="lg-rotate-right lg-icon"></button>';
        }
        this.core.outer.querySelector('.lg-toolbar').insertAdjacentHTML('beforeend', rotateIcons);
    };

    Rotate.prototype.init = function () {
        var _this = this;
        this.buildTemplates();

        // Save rotate config for each item to persist its rotate, flip values
        // even after navigating to diferent slides
        this.rotateValuesList = {};

        // event triggered after appending slide content
        utils.on(_this.core.el, 'onAferAppendSlide.lgtmrotate', function (event) {
            // Get the current element
            var imageWrap = _this.core.___slide[event.detail.index].querySelector('.lg-img-wrap');
            utils.wrap(imageWrap, 'lg-img-rotate');
        });

        utils.on(_this.core.outer.querySelector('.lg-rotate-left'), 'click.lg', this.rotateLeft.bind(this));
        utils.on(_this.core.outer.querySelector('.lg-rotate-right'), 'click.lg', this.rotateRight.bind(this));
        utils.on(_this.core.outer.querySelector('.lg-flip-hor'), 'click.lg', this.flipHorizontal.bind(this));
        utils.on(_this.core.outer.querySelector('.lg-flip-ver'), 'click.lg', this.flipVertical.bind(this));

        // Reset rotate on slide change
        utils.on(_this.core.el, 'onBeforeSlide.lgtmrotate', function (event) {
            if (!_this.rotateValuesList[event.detail.index]) {
                _this.rotateValuesList[event.detail.index] = {
                    rotate: 0,
                    flipHorizontal: 1,
                    flipVertical: 1
                };
            }
        });
    };

    Rotate.prototype.applyStyles = function () {
        var image = this.core.___slide[this.core.index].querySelector('.lg-img-rotate');
        utils.setVendor(image, 'Transform', 'rotate(' + this.rotateValuesList[this.core.index].rotate + 'deg)' + ' scale3d(' + this.rotateValuesList[this.core.index].flipHorizontal + ', ' + this.rotateValuesList[this.core.index].flipVertical + ', 1)');
    };

    Rotate.prototype.rotateLeft = function () {
        this.rotateValuesList[this.core.index].rotate -= 90;
        this.applyStyles();
    };

    Rotate.prototype.rotateRight = function () {
        this.rotateValuesList[this.core.index].rotate += 90;
        this.applyStyles();
    };

    Rotate.prototype.getCurrentRotation = function (el) {
        if (!el) {
            return 0;
        }
        var st = window.getComputedStyle(el, null);
        var tm = st.getPropertyValue('-webkit-transform') || st.getPropertyValue('-moz-transform') || st.getPropertyValue('-ms-transform') || st.getPropertyValue('-o-transform') || st.getPropertyValue('transform') || 'none';
        if (tm !== 'none') {
            var values = tm.split('(')[1].split(')')[0].split(',');
            if (values) {
                var angle = Math.round(Math.atan2(values[1], values[0]) * (180 / Math.PI));
                return angle < 0 ? angle + 360 : angle;
            }
        }
        return 0;
    };

    Rotate.prototype.flipHorizontal = function () {
        var rotateEl = this.core.___slide[this.core.index].querySelector('.lg-img-rotate');
        var currentRotation = this.getCurrentRotation(rotateEl);
        var rotateAxis = 'flipHorizontal';
        if (currentRotation === 90 || currentRotation === 270) {
            rotateAxis = 'flipVertical';
        }
        this.rotateValuesList[this.core.index][rotateAxis] *= -1;
        this.applyStyles();
    };

    Rotate.prototype.flipVertical = function () {
        var rotateEl = this.core.___slide[this.core.index].querySelector('.lg-img-rotate');
        var currentRotation = this.getCurrentRotation(rotateEl);
        var rotateAxis = 'flipVertical';
        if (currentRotation === 90 || currentRotation === 270) {
            rotateAxis = 'flipHorizontal';
        }
        this.rotateValuesList[this.core.index][rotateAxis] *= -1;

        this.applyStyles();
    };

    Rotate.prototype.destroy = function () {
        // Unbind all events added by lightGallery rotate plugin
        utils.off(this.core.el, '.lgtmrotate');
        this.rotateValuesList = {};
    };

    window.lgModules.Rotate = Rotate;
});

},{}]},{},[1])(1)
});
