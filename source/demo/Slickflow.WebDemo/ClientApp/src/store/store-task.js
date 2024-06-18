import axios from 'axios'
import kmsgbox from '../js/kmsgbox'
import config from '../config/index.js'

const state = {
  todotasks: null,
  donetasks: null,
  createdbymetasks: null,
  selectedtask: null
}

const getters = {
  todotasks: state => {
    return state.todotasks
  },
  donetasks: state => {
    return state.donetasks
  },
  createdbymetasks: state => {
    return state.createdbymetasks
  },
  selectedtask: state => {
    return state.selectedtask
  }
}

const mutations = {
  setToDo: (state, payload) => {
    state.todotasks = payload
  },
  setDone: (state, payload) => {
    state.donetasks = payload
  },
  setCreated: (state, payload) => {
    state.createdbymetasks = payload
  },
  setSelected: (state, payload) => {
    state.selectedtask = payload
  }
}

const actions = {
  getToDo: (context, payload) => {
    var user = payload.user
    if (user === null) return

    var callback = payload.callback

    var query = {}
    query.ProcessGUID = config.PROCESS_A4L.ProcessGUID
    query.Version = config.PROCESS_A4L.Version
    query.UserID = user.UserID
    query.UserName = user.UserName

    axios.post('/webdemoapi/api/wf/QueryReadyTasks', query)
      .then((response) => {
        var todotasks = response.data.Entity
        context.commit('setToDo', todotasks)
        if (callback) callback()
      })
      .catch((error) => {
        window.console.log(error)
      })
  },
  getDone: (context, payload) => {
    var user = payload.user
    if (user !== null) {
      var callback = payload.callback

      var query = {}
      query.ProcessGUID = config.PROCESS_A4L.ProcessGUID
      query.Version = config.PROCESS_A4L.Version
      query.UserID = user.UserID
      query.UserName = user.UserName

      axios.post('/webdemoapi/api/wf/QueryCompletedTasks', query)
        .then((response) => {
          var donetasks = response.data.Entity
          context.commit('setDone', donetasks)
          if (callback) callback()
        })
        .catch((error) => {
          window.console.log(error)
        })
    }
  },
  getCreated: (context, payload) => {
    var query = {}
    query.ProcessGUID = config.PROCESS_A4L.ProcessGUID
    query.Version = config.PROCESS_A4L.Version

    axios.post('/webdemoapi/api/wf/QueryCreatedTasks', query)
      .then((response) => {
        var donetasks = response.data.Entity
        context.commit('setDone', donetasks)
      })
      .catch((error) => {
        window.console.log(error)
      })
  },
  setSelectedTask: (context, payload) => {
    context.commit('setSelected', payload)
  },
  rejectTask: (context, payload) => {
    var user = payload.user
    var task = payload.task
    var callback = payload.callback

    var query = {}
    query.ProcessGUID = config.PROCESS_A4L.ProcessGUID
    query.Version = config.PROCESS_A4L.Version
    query.UserID = user.UserID
    query.UserName = user.UserName
    query.AppName = task.AppName
    query.AppInstanceID = task.AppInstanceID
    query.TaskID = task.TaskID

    axios.post('/webdemoapi/api/wf/RejectProcess', query)
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
  }
}

export default {
  namespaced: true,
  state,
  mutations,
  actions,
  getters
}
