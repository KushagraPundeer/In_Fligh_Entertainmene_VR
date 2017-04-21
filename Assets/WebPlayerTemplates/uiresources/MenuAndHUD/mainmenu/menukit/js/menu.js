/*
This file is part of Coherent UI, modern user interface library for
games. Release $RELEASE$. Build $VERSION$ for $LICENSEE$.

Copyright (c) 2012-2014 Coherent Labs AD and/or its licensors. All
rights reserved in all media.

The coded instructions, statements, computer programs, and/or related
material (collectively the "Data") in these files contain confidential
and unpublished information proprietary Coherent Labs and/or its
licensors, which is protected by United States of America federal
copyright law and by international treaties.

This software or source code is supplied under the terms of a license
agreement and nondisclosure agreement with Coherent Labs Limited and may
not be copied, disclosed, or exploited except in accordance with the
terms of that agreement. The Data may not be disclosed or distributed to
third parties, in whole or in part, without the prior written consent of
Coherent Labs Limited.

COHERENT LABS MAKES NO REPRESENTATION ABOUT THE SUITABILITY OF THIS
SOURCE CODE FOR ANY PURPOSE. THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT
HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES,
INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF
MERCHANTABILITY, NONINFRINGEMENT, AND FITNESS FOR A PARTICULAR PURPOSE
ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER, ITS AFFILIATES,
PARENT COMPANIES, LICENSORS, SUPPLIERS, OR CONTRIBUTORS BE LIABLE FOR
ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS
OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION)
HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT,
STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN
ANY WAY OUT OF THE USE OR PERFORMANCE OF THIS SOFTWARE OR SOURCE CODE,
EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/
/*!
* Coherent Menu Kit allows you to create quick and easy menu for your game.
*
* Simple usages
* Firstly you have to declare the buttons of your menu.
* To do that you have to create an array with objects with following
* properties:
* label: {string} name of the button
* style: {string} this is a style name of the button. By default style
* name is 'primary'. You can check for all buttons styling in examples
* of Flat-UI library at http://greensock.com/jump-start-js#easing
* callback: {function} takes function for parameter. This function will be
* execute when button is clicked.
*
* Here is some example of buttons declaration.
*
* var newGameButtons = [
* {label: 'Easy', style: 'success', callback: easy},
* {label: 'Medium', style: 'info', callback: medium},
* {label: 'Hard', style: 'danger', callback: hard},
* {label: 'Back',
*   callback: function() {swapMenus(newGameMenu, startMenu);}
* }
* ];
*/

(function() {

  'use strict';

  var parentsCollection = [];

  /*!
  * @constructor GameMenu
  * @param {object} optionsObj
  * @property {array} optionsObj.buttons: here you have to put your buttons
  * array whit objects
  * @property {(object|string)} [optionsObj.originPos=center]
  * optionsObj.originPos: by default position of menu will be in  the center.
  * You can pass an object with position. For example {top: 100, left: 100};.
  * That will move the menu 100px from top and 100px from left positions,
  * starting from your parent element. Other way is to send a string. The
  * string can be - top-left, top-right, center-top, center-left, center-right,
  * center, center-bottom, bottom-left, bottom-right. You can create menus of
  * all this direction in your stage.
  * @property {(object|string)}{bool} [optionsObj.fixedOffset=false]
  * optionsObj.fixedOffset: by default fixedOffset is false. The false meant
  * that units for menu position will be in percent. If its true units will
  * be in pixels
  * @property {(object|string)}
  * [optionsObj.parent=document.getElementsByTagName('body')[0]]
  * optionsObj.parent: by default if is not pass, parent element will be body
  * of the document. You can set your parent element for menu by sending a
  * string selector like "#content .inner" or object selector like
  * document.getElementById('content');. That will be the parent element
  * of who will be append the menu.
  */
  var GameMenu = function(optionsObj) {

    this.buttons = optionsObj.buttons;
    this.originPos = optionsObj.originPos || 'center';
    this.fixedOffset = optionsObj.fixedOffset || false;
    this._isParentSend = true;
    this.parentMenuHtml = findElement(this, optionsObj.parent);

    this._createMenu();
    return this;
  };

  GameMenu.prototype._createMenu = function() {

    // Check if parent element exist
    if (!(checkArrElExist(parentsCollection, this.parentMenuHtml))) {
      var parentToPush = {
        isMenuCreated: false,
        name: this.parentMenuHtml
      };
      parentsCollection.push(parentToPush);
    }

    // Create div element for menu
    var menuHtml = document.createElement('div');
    menuHtml.className = 'cui-menu';

    // html scope of menu
    this.menuHtml = menuHtml;

    // Save original reference to menu if is changed
    this.originMenuHtml = this.menuHtml;

    var buttonsLen = this.buttons.length;

    for (var i = 0; i < buttonsLen; i++) {
      var buttonOption = this.buttons[i];

      // Generate HTML for buttons
      var buttonHtml = this._buildButton(buttonOption);

      // Append button to menu
      this.menuHtml.appendChild(buttonHtml);
    }

    // Generate custom styles and classes for menu
    this._generateMenuStyle(this.originPos, this.fixedOffset);

    this._wrapCreate();

    // Append menu to parent
    this.parentMenuHtml.appendChild(this.menuHtml);
  };

  GameMenu.prototype._wrapCreate = function() {

    if (!checkIsMenuCreated(parentsCollection, this.parentMenuHtml)) {

      // Create global wrap
      var wrapGlobalHtml = document.createElement('div');
      wrapGlobalHtml.className = 'cui-wrapper';

      // If parent element is not sent then set id in global wrap
      if (!this._isParentSend) {
        wrapGlobalHtml.id = 'cui-wrapper';
      }

      // Create div element for menu wrap
      var wrapMenuHtml = document.createElement('div');
      wrapMenuHtml.className = 'cui-wrapper-menu';

      wrapMenuHtml.appendChild(this.menuHtml);
      wrapGlobalHtml.appendChild(wrapMenuHtml);

      // For first time append push menu with wraps
      this.menuHtml = wrapGlobalHtml;
    } else {
      // Refer parent to global wrap
      var qSelectorAll =  document.querySelectorAll.bind(document);
      this.parentMenuHtml = qSelectorAll('#cui-wrapper .cui-wrapper-menu')[0];
    }
  };

  GameMenu.prototype._buildButton = function(options) {

    // Create button element for buttons
    var buttonHtml = document.createElement('button');
    var style = options.style || 'primary';
    buttonHtml.type = 'button';
    buttonHtml.classList.add('btn', 'btn-large', 'btn-' + style);

    // Button name
    buttonHtml.innerHTML = options.label;

    // Attach click event on every button
    buttonHtml.addEventListener('click', options.callback, false);

    // Set button to disabled if is passed
    if (options.disabled === true)
    {
      buttonHtml.setAttribute('disabled', 'disabled');
    }
    return buttonHtml;
  };

  GameMenu.prototype._generateMenuStyle = function(options, isFixedPos) {

    var unit;
    this.menuHtml.classList.add('cui-default');

    if (isFixedPos) {
      unit = 'px';
    } else {
      unit = '%';
      this.menuHtml.classList.add('percent');
    }

    if (typeof options === 'object') {

      for (var property in options) {
        this.menuHtml.style[property] = options[property] + unit;
      }

    } else {
      this.menuHtml.classList.add(options);
    }
  };

  GameMenu.prototype.show = function() {
    this.originMenuHtml.style.display = 'block';
    return this;
  };

  GameMenu.prototype.hide = function() {
    this.originMenuHtml.style.display = 'none';
    return this;
  };

  function findElement(menu, selector) {
    if (typeof selector === 'string') {
      return document.querySelectorAll(selector)[0];
    } else if (selector === undefined) {
      menu._isParentSend = false;
      return document.getElementsByTagName('body')[0];
    } else {
      return selector;
    }
  }

  function checkArrElExist(arr, el) {
    for (var i = 0; i < arr.length; i++) {
      if (arr[i].name === el) {
        return true;
      }
    }
    return false;
  }

  function checkIsMenuCreated(arr, el) {
    for (var i = 0; i < arr.length; i++) {
      if (arr[i].name === el && arr[i].isMenuCreated === false) {
        arr[i].isMenuCreated = true;
        return false;
      }
    }
    return true;
  }

  /*!
  * export to AMD/RequireJS and Node.js or created CoherentAnimation by
  * standard way including scripts tags in HTML
  */
  if (typeof define === 'function' && define.amd) {
    if (typeof require === 'function' && require !== undefined) {
      require.config({
        // map require-css plug-in to load css files
        map: {
          '*': {
            'css': 'components/require-css/css'
          }
        }
        // Require flatui and menu css files for game menu
      });
    }

    define('GameMenu', ['css!lib/css/main.css'], function() {
      return GameMenu;
    });
  }

  window.GameMenu = GameMenu;
})();