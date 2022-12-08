import Vue from 'vue'
import Vuex from 'vuex'

import ru from './store-ru'
import leave from './store-leave'
import task from './store-task'
import account from './store-account'
import kres from '../js/kres.js'

Vue.use(Vuex)

/*
 * If not building with SSR mode, you can
 * directly export the Store instantiation
 */

export default function (/* { ssrContext } */) {
  const Store = new Vuex.Store({
    modules: {
      ru,
      leave,
      task,
      account,
      kres
    },

    // enable strict mode (adds overhead!)
    // for dev mode only
    strict: process.env.DEV
  })

  return Store
}
