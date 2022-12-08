import axios from 'axios'
import kmsgbox from '../js/kmsgbox'
import config from '../config/index.js'

const state = {
  currentLeave: null
}

const getters = {
  currentLeave: state => {
    return state.currentLeave
  }
}

const mutations = {
  setCurrent: (state, payload) => {
    state.currentLeave = payload
  }
}

const actions = {
  getLeave: (context, payload) => {
    var leaveID = payload.leaveID
    var callback = payload.callback
    axios.get('/webdemoapi/api/HrsLeave/GetLeave/' + leaveID)
      .then((response) => {
        var entity = response.data.Entity
        context.commit('setCurrent', entity)
        if (callback) callback()
      })
      .catch((error) => {
        window.console.log(error)
      })
  },
  queryLeave: (context, payload) => {
    var user = payload.user
    if (user !== null) {
      var callback = payload.callback
      var query = {
        CreatedUserID: user.UserID
      }
      axios.post('/webdemoapi/api/HrsLeave/queryLeave/', query)
        .then((response) => {
          var leaveList = response.data.Entity
          if (callback) callback(leaveList)
        })
        .catch((error) => {
          window.console.log(error)
        })
    }
  },
  submitLeave: (context, payload) => {
    var entity = payload.entity
    entity.FromDate = new Date(entity.FromDate)

    const user = payload.user
    entity.CreatedUserID = user.UserID
    entity.CreatedUserName = user.UserName

    var runner = {
      ProcessGUID: config.PROCESS_A4L.ProcessGUID,
      Version: config.PROCESS_A4L.Version,
      AppName: config.PROCESS_A4L.AppName,
      UserID: user.UserID,
      UserName: user.UserName
    }

    var entityRunner = {
      Entity: entity,
      Runner: runner
    }

    var callback = payload.callback
    axios.post('/webdemoapi/api/HrsLeave/CreateNewLeave', entityRunner)
      .then((response) => {
        var result = response.data
        if (result.Status === 1) {
          if (callback) callback()
        } else {
          kmsgbox.error(result.Message)
        }
      })
      .catch((error) => {
        window.console.log(error)
      })
  },
  saveLeave: (context, payload) => {
    var entity = payload.Leave
    var callback = payload.callback
    axios.post('/webdemoapi/api/HrsLeave/UpdateLeave', entity)
      .then((response) => {
        var result = response.data
        if (callback !== null) callback(result)
      })
      .catch((error) => {
        window.console.log(error)
      })
  }
}

export default {
  namespaced: true,
  state,
  mutations,
  actions,
  getters
}
