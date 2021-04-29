/*
* Slickflow 工作流引擎遵循LGPL协议，也可联系作者商业授权并获取技术支持；
* 除此之外的使用则视为不正当使用，请您务必避免由此带来的商业版权纠纷。

The Slickflow Designer project.
Copyright (C) 2014  .NET Workflow Engine Library

This library is free software; you can redistribute it and/or
modify it under the terms of the GNU Lesser General Public
License as published by the Free Software Foundation; either
version 2.1 of the License, or (at your option) any later version.

This library is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public
License along with this library; if not, you can access the official
web page about lgpl: https://www.gnu.org/licenses/lgpl.html
*/
import kres from '../js/kres.js'
import { Dialog } from 'quasar'

const kmsgbox = (function () {
  function kmsgbox () {

  }

  kmsgbox.info = function (message) {
    Dialog.create({
      title: kres.get('info'),
      message: message,
      html: true
    }).onOk(() => {
      // console.log('OK')
    }).onCancel(() => {
      // console.log('Cancel')
    }).onDismiss(() => {
      // console.log('I am triggered on both OK and Cancel')
    })
  }

  kmsgbox.warn = function (message) {
    Dialog.create({
      title: kres.get('warning'),
      message: message
    }).onOk(() => {
      // console.log('OK')
    }).onCancel(() => {
      // console.log('Cancel')
    }).onDismiss(() => {
      // console.log('I am triggered on both OK and Cancel')
    })
  }

  kmsgbox.error = function (message) {
    Dialog.create({
      title: kres.get('error'),
      message: message
    }).onOk(() => {
      // console.log('OK')
    }).onCancel(() => {
      // console.log('Cancel')
    }).onDismiss(() => {
      // console.log('I am triggered on both OK and Cancel')
    })
  }

  kmsgbox.confirm = function (message) {
    Dialog.create({
      title: kres.get('confirm'),
      message: message,
      cancel: true,
      persistent: true
    }).onOk(() => {
      // console.log('>>>> OK')
    }).onOk(() => {
      // console.log('>>>> second OK catcher')
    }).onCancel(() => {
      // console.log('>>>> Cancel')
    }).onDismiss(() => {
      // console.log('I am triggered on both OK and Cancel')
    })
  }

  kmsgbox.prompt = function (message) {
    Dialog.create({
      title: kres.get('prompt'),
      message: message,
      prompt: {
        model: '',
        type: 'text' // optional
      },
      cancel: true,
      persistent: true
    }).onOk(data => {
      // console.log('>>>> OK, received', data)
    }).onCancel(() => {
      // console.log('>>>> Cancel')
    }).onDismiss(() => {
      // console.log('I am triggered on both OK and Cancel')
    })
  }

  return kmsgbox
})()

export default kmsgbox
