(function ($) {
    $.fn.editableSelect = function (method) {
        if (methods[method]) {
            return methods[method].apply(this, Array.prototype.slice.call(arguments, 1));
        } else if (typeof method === 'object' || !method) {
            return methods.init.apply(this, arguments);
        } else {
            $.error('Method: ' + method + ' does not exist in jQuery.editableSelect.js');
        }
    }

    $.fn.editableSelect.defaults = {
        enableiframe: false
    };

    var methods = {
        init: function (options) {
            var settings = $.fn.editableSelect.defaults;
            if (options) {
                $.extend(settings, options);
            }
            return $(this).each(function create() {
                var $element = $(this);
                $element.data('editableSelect', {
                    editableSelect: new EditableSelect($element, settings)
                });
            });
        },
        update: function () {
            return this.each(function update() {
                var instance = getInstance(this);
                instance.container.contents().remove();
                instance.buildList();
            });
        },
        enable: function () {
            return this.each(function enable() {
                getInstance(this).enable();
            });
        },
        disable: function () {
            return this.each(function disable() {
                getInstance(this).disable();
            });
        }
    };

    function getInstance(element) {
        var data = $(element).data('editableSelect');
        if (data) {
            return data.editableSelect;
        }
        $.error('editableSelect not initialized.');
    };

    function EditableSelect($element, options) {
        this.init($element, options);
    };

    EditableSelect.prototype = {
        $textbox: {},
        $select: {},
        $container: {},
        $iframe: false,
        $listItems: false,
        value: '',
        index: -1,
        liValues: {},
        options: {},
        init: function ($element, options) {
            this.options = options;
            this.$select = $element;
            var $clone = this.$select.clone(true);
            $clone.css({ display: 'block', top: '-1000' });
            $clone.appendTo('body');

            $("div.esContainer").remove();

            if ($.isEmptyObject(this.$container) == true) {
                this.$container = $('<div class="esContainer" style="z-index:99999;" />')
                    .appendTo(document.body);
            }

            this.$textbox = $('<input type="text" class="esTextBox" style="min-width:45px;">')
                .css({
                    width: '100%',
                    'margin-left':'-2px'
                    //height: $clone.outerHeight()
                })
                .attr('tabindex', this.$select.attr('tabindex'));

            this.$textbox
                .appendTo(this.$select.parent());

            this.buildList();
            this.$select.hide();
            $clone.remove();
            this._hideList();
            this._bindEvents();
            if (this.$select.is(':disabled')) {
                this.disable();
            }
            this.tester = true;
        },
        enable: function () {
            this.$textbox.removeAttr('disabled');
        },
        disable: function () {
            this.$textbox.attr('disabled', 'disabled');
        },
        buildList: function () {
            var self = this;
            var selected = false;
            var $list = $('<ul class="ui-autocomplete ui-menu ui-widget ui-widget-content ui-corner-all">')
                .css({
                    'list-style-type': 'none',
                    padding: 0,
                    margin: 0,
                    width:'100%'
                })
                .appendTo(self.$container);

            self.liValues = [];
            self.$listItems = $(self.$select.children('option')
                .map(function buildOptions(idx, element) {
                    var $element = $(element);
                    var text = $element.text();
                    if ($element.is(':selected')) {
                        self.value = text;
                        selected = true;
                    }
                    var li = $("<li/>", { text: text });
                    if (selected) {
                        self.index = idx;
                        selected = false;
                    }
                    self.liValues.push(text);
                    return li.get();
                }))
                .appendTo($list);

            self.$textbox.val(self.value);
            self.$listItems.addClass('esLi esItemHover');

            var autoTexts = jQuery.map(self.$listItems, function (x) {
                return x.innerHTML;
            });
            self.$textbox.autocomplete({
                source: autoTexts
            });
        },
        _bindEvents: function () {
            var self = this;
            this.$textbox.click(function tbClick(e) {
                e.stopPropagation();
                if (self.$container.is(':visible')) {
                    self._hideList();
                } else {
                    $(document.body).trigger('jqes.closeAll');
                    self._showList();
                }
            }).keydown(function tbKeydown(e) {
                switch (e.keyCode) {
                    case 9://Tab
                    case 13://Enter
                        var visible = self.$container.is(':visible');
                        if (visible) {
                            self._selectItem(self._getCurrentLiDiv().text());
                        }
                        if (e.keyCode == 13) {
                            e.preventDefault();
                            $(this).trigger('change');
                            return true;
                        }
                        break;
                    case 38://Up
                        e.preventDefault();
                        self._moveSelection('up');
                        break;
                    case 40://Down
                        if (self.$container.is(':visible')) {
                            e.preventDefault();
                        } else {
                            self._showList();
                        }
                        self._moveSelection('down');
                        break;
                    case 27://Esc
                        e.preventDefault();
                        self.$textbox.val(self.value);
                        self._cancelSelection();
                    case 46://Del
                        e.preventDefault();
                        self.$textbox.val(self.value);
                        self.$textbox.data("isDel", true);
                    default:
                        //add auto complete or filtering
                        self._hideList();
                }
            }).change(function tbChange(e) {
                var value = $(this).val();
                if (value === self.value) {
                    return false;
                }
                self.index = self._indexOf(self.liValues, value);
                var editableOption = $('option[data-jqes="editable"]', self.$select);
                if (self.index === -1) {
                    if (editableOption.length === 0) {
                        editableOption = $('<option>' + value + '</option>').attr({
                            'data-jqes': 'editable',
                            value: value
                        });
                        self.$select.append(editableOption);
                    } else {
                        editableOption.text(value);
                    }
                }
                self._selectItem(value);
            });
            $(document.body).bind('click.jqes jqes.closeAll', function bodyClickClose(e) {
                if (e.target !== self.$textbox.get(0) && !self.$container.has(e.target).length) {
                    self._cancelSelection();
                }
            });
            self.$listItems.each(function bindLiEvents() {
                $(this).hover(function lihover() {
                    self._getCurrentLiDiv().removeClass('esItemHover');
                    $(this).toggleClass('esItemHover');
                });
                $(this).mouseup(function liMouseUp(e) {
                    if (e.target === this) {
                        self._selectItem($(this).text());
                        e.stopPropagation();
                    }
                });
            });
        },
        _showList: function () {
            if (this.$listItems.length === 0) {
                return;
            }
            this.$container.show();
            var viewHeight = $(window).height();
            var scrollTop = $(window).scrollTop();
            var textHeight = this.$textbox.outerHeight();
            var itemsHeight = $(this.$listItems[0]).parent().outerHeight();
            var containerHeight = itemsHeight;
            var offset = this.$textbox.offset();
            var spaceUp = offset.top - scrollTop;
            var spaceDown = viewHeight - (spaceUp + textHeight);
            var verticalMax = spaceUp > spaceDown ? spaceUp : spaceDown;
            if (itemsHeight > verticalMax) {
                containerHeight = verticalMax;
            }
            if ((spaceUp > spaceDown) && (containerHeight > spaceDown)) {
                offset.top = offset.top - containerHeight;
            } else {
                offset.top = offset.top + textHeight;
            }
            this.$container.css({
                height: containerHeight,
                width: this.$select.outerWidth(),
                'min-width':this.$textbox.outerWidth(),
                'overflow-y': function overflow() {
                    return itemsHeight > containerHeight ? 'auto' : 'visible';
                }
            });
            // Double for IE bug
            this.$container.offset(offset);
            this.$container.offset(offset);
            this._createiframe();
        },
        _hideList: function () {
            this.$container.hide();
            if (this.$iframe) {
                this.$iframe.detach();
            }
        },
        _getCurrentLiDiv: function () {
            var index = this.index;
            return (index > -1 && index < this.$listItems.length) ? $(this.$listItems.get(index)) : $();
        },
        _cancelSelection: function () {
            this._hideList();
            this.index = this._indexOf(this.liValues, this.value);
            this._hilightItem(0);
        },
        _selectItem: function (value) {
            var self = this;
            self._hideList();
            this.value = value;
            this.$select.val(value);
            this.$textbox.val(value);
            this.$select.change();
            this.index = self._indexOf(this.liValues, value);
            self._getCurrentLiDiv().addClass('esItemHover');
            this.$textbox.focus().select();
            self.$textbox.data("isSelected", true);
        },
        _moveSelection: function (direction) {
            if (direction === 'down' && this.index < (this.$listItems.length - 1)) {
                this._hilightItem(+1);
            } else if (direction === 'up' && this.index > -1) {
                this._showList();
                this._hilightItem(-1);
                if (this.index === -1) {
                    this.$textbox.focus();
                    this._hideList();
                }
            }
        },
        _hilightItem: function (increment) {
            this.$listItems.removeClass('esItemHover');
            this.index = this.index + increment;
            this._getCurrentLiDiv().addClass('esItemHover');
        },
        _indexOf: function (array, value) {
            var length = array.length;
            for (var i = 0; i < length; i++) {
                if (array[i] === value) {
                    return i;
                }
            }
            return -1;
        },
        _createiframe: function () {
            if (!this.options.enableiframe) {
                return;
            }
            var offset = this.$container.offset();
            if (!this.$iframe) {
                this.$iframe = $('<iframe  src="javascript:false;"  tabindex="-1" frameborder="0" style="position:abolsute;' +
                                 ' display:inline; z-index=-1; filter:Alpha(opacity=\'0\'); width:' + this.$container.outerWidth() + 'px; " />');
            }
            this.$iframe.css({
                left: offset.left - 1,
                top: offset.top - 1,
                height: this.$container.outerHeight()
            });
            this.$iframe.appendTo(this.$container);
        }
    };
})(jQuery);
