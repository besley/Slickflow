import { LocalStorage } from 'quasar'

const state = {
  user: null,
  role: null
}

const getters = {
  loginUser: state => {
    var user = LocalStorage.getItem('CURRENT_LOGIN_USER')
    return user
  },
  loginRole: state => {
    var role = LocalStorage.getItem('CURRENT_LOGIN_ROLE')
    return role
  }
}

const mutations = {
  setAccount: (state, payload) => {
    state.user = payload.User
    state.role = payload.Role

    LocalStorage.set('CURRENT_LOGIN_ROLE', state.role)
    LocalStorage.set('CURRENT_LOGIN_USER', state.user)
  }
}

const actions = {
  saveAccount: (context, payload) => {
    context.commit('setAccount', payload)
  }
}

export default {
  namespaced: true,
  state,
  mutations,
  actions,
  getters
}
