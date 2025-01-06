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
