<template>
  <q-dialog ref="dialog" @hide="onDialogHide">
    <q-card class="q-dialog-plugin">
      <!--
    ...content
    ... use q-card-section for it?
  -->
      <q-card-section style="height:400px;">
        <h4>{{ui.steptitle}}</h4>
        <q-tree class="col-12 col-sm-6"
                :nodes="steps"
                node-key="label"
                tick-strategy="leaf-filtered"
                :ticked.sync="ticked"
                :expanded.sync="expanded"
                ref="nextsteptree"/>
      </q-card-section>

      <!-- buttons example -->
      <q-card-actions align="right">
        <q-btn color="primary" label="OK" @click="onOKClick" />
        <q-btn color="primary" label="Cancel" @click="onCancelClick" />
      </q-card-actions>
    </q-card>
  </q-dialog>
</template>

<script>
import axios from 'axios'
import $ from 'jquery'
import { mapGetters } from 'vuex'
import kmsgbox from '../../js/kmsgbox.js'

export default {
  name: 'NextStepTree',
  props: ['task', 'conditions'],
  created () {
    var self = this
    this.getNextSteps(function (steps) {
      self.steps = steps
      self.$nextTick(() => { self.$refs.nextsteptree.expandAll() })
    })
  },
  data () {
    return {
      ui: {
        steptitle: this.gmxGetTitle('steptitle')
      },
      steps: [],
      ticked: null,
      expanded: null
    }
  },
  methods: {
    // following method is REQUIRED
    // (don't change its name --> "show")
    ...mapGetters('account', ['loginUser']),
    show () {
      this.$refs.dialog.show()
    },

    // following method is REQUIRED
    // (don't change its name --> "hide")
    hide () {
      this.$refs.dialog.hide()
    },

    onDialogHide () {
      // required to be emitted
      // when QDialog emits "hide" event
      this.$emit('hide')
    },

    onOKClick () {
      // run process
      this.runProcess()

      // on OK, it is REQUIRED to
      // emit "ok" event (with optional payload)
      // before hiding the QDialog
      this.$emit('ok')
      // or with payload: this.$emit('ok', { ... })

      // then hiding dialog
      this.hide()
    },

    onCancelClick () {
      // we just need to hide dialog
      this.hide()
    },
    getNextSteps (callback) {
      var task = this.task
      var query = {}
      query.ProcessGUID = task.ProcessGUID
      query.Version = task.Version
      query.AppInstanceID = task.AppInstanceID
      query.TaskID = task.TaskID
      query.Conditions = this.conditions

      axios.post('/webdemoapi/api/wf/GetNextStepInfo', query)
        .then((response) => {
          var result = response.data
          if (result.Status === 1) {
            var steps = result.Entity.NextActivityRoleUserTree
            var treeStepNodes = []
            this.makeTreeStepNodes(steps, treeStepNodes)
            // render tree control
            if (callback) callback(treeStepNodes)
          } else {
            kmsgbox.error(result.Message)
          }
        })
        .catch((error) => {
          window.console.log(error)
        })
    },
    makeTreeStepNodes (steps, treeStepNodes) {
      var root = { label: 'Next Step', children: [], type: 'root' }
      treeStepNodes.push(root)
      $.each(steps, function (index, step) {
        var child = { label: step.ActivityName, id: step.ActivityGUID, children: [], type: 'activity' }
        $.each(step.Users, function (index, user) {
          var u = { label: user.UserName, id: user.UserID, activity: step.ActivityGUID, type: 'user' }
          child.children.push(u)
        })
        root.children.push(child)
      })
    },
    runProcess () {
      // goforward the process
      var performers = {}

      var tickedNodes = this.$refs.nextsteptree.getTickedNodes()
      $.each(tickedNodes, function (index, node) {
        if (node.type === 'user') {
          appendUser(performers, node)
        } else if (node.type === 'activity') {
          appendActivity(performers, node)
        } else {
          kmsgbox.warn(this.gmxGetTitle('NextStepTree.runProcess.warn'))
        }
      })

      function appendUser (performers, node) {
        var activity = node.activity
        if (performers[activity] === undefined) performers[activity] = []
        var user = { UserID: node.id, UserName: node.label }
        performers[activity].push(user)
      }

      function appendActivity (performers, node) {
        var activity = node.id
        if (performers[activity] === undefined) performers[activity] = []
        performers[activity] = []
      }

      var task = this.task
      var runner = {}
      runner.ProcessGUID = task.ProcessGUID
      runner.Version = task.Version
      runner.AppName = task.AppName
      runner.AppInstanceID = task.AppInstanceID
      runner.TaskID = task.TaskID
      runner.NextActivityPerformers = performers

      var user = this.loginUser()
      runner.UserID = user.UserID
      runner.UserName = user.UserName
      runner.Conditions = this.conditions

      axios.post('/webdemoapi/api/wf/RunProcess', runner)
        .then((response) => {
          var result = response.data
          if (result.Status === 1) {
            kmsgbox.info(this.gmxGetTitle('NextStepTree.runProcess.ok'))
          } else {
            kmsgbox.error(result.Message)
          }
        })
        .catch((error) => {
          window.console.log(error)
        })
    }
  }
}
</script>
