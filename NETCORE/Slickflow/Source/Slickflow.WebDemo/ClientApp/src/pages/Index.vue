<template>
  <q-page>
    <div class="row" style="margin-bottom:10px;margin-top:10px;">
      <q-btn color="secondary" icon="add" :label="ui.apply" @click="onTaskCreate"></q-btn>
      <q-btn color="orange" icon="arrow_forward" :label="ui.forward" @click="onTaskEdit"></q-btn>
      <q-btn color="green" icon="insights" :label="ui.diagram" @click="onDiagramShow"></q-btn>
      <q-btn color="red" icon="repeat" :label="ui.reject" @click="onReject"></q-btn>
    </div>
    <q-card padding>
      <q-tabs v-model="tab"
              dense
              class="text-grey"
              active-color="primary"
              indicator-color="primary"
              align="justify"
              narrow-indicator>
        <q-tab icon="mail" name="todo" :label="ui.todolist" />
        <q-tab icon="anchor" name="done" :label="ui.done" />
        <q-tab icon="manage_accounts" name="myleave" :label="ui.myleave" />
      </q-tabs>

      <q-separator />

      <q-tab-panels v-model="tab" animated>
        <q-tab-panel name="todo">
          <todo ref="mytodotasks" />
        </q-tab-panel>

        <q-tab-panel name="done">
          <done />
        </q-tab-panel>

        <q-tab-panel name="myleave">
          <myleave />
        </q-tab-panel>
      </q-tab-panels>
    </q-card>
  </q-page>
</template>

<script>
import { mapActions, mapGetters } from 'vuex'
import kmsgbox from '../js/kmsgbox.js'

export default {
  name: 'Index',
  data () {
    return {
      ui: {
        apply: this.gmxGetTitle('apply'),
        approval: this.gmxGetTitle('approval'),
        forward: this.gmxGetTitle('forward'),
        todolist: this.gmxGetTitle('todolist'),
        myleave: this.gmxGetTitle('myleave'),
        done: this.gmxGetTitle('done'),
        diagram: this.gmxGetTitle('diagram'),
        sendback: this.gmxGetTitle('sendback'),
        resend: this.gmxGetTitle('resend'),
        reject: this.gmxGetTitle('reject')
      },
      tab: 'todo',
      selectedToDoTask: null,
      baseUrl: process.env.VUE_APP_BASE_URL
    }
  },
  components: {
    todo: require('components/Task/ToDo.vue').default,
    done: require('components/Task/Done.vue').default,
    myleave: require('components/Leave/MyLeave.vue').default
  },
  methods: {
    ...mapActions('task', ['setSelectedTask', 'rejectTask']),
    ...mapActions('leave', ['getLeave']),
    ...mapGetters('account', ['loginUser']),
    onTaskCreate () {
      var user = this.loginUser()
      if (user === null) {
        kmsgbox.warn(this.gmxGetTitle('Index.Account.notlogin.warn'))
      } else {
        this.$router.push('/leave/add')
      }
    },
    onTaskEdit () {
      if (this.$refs.mytodotasks !== undefined &&
          this.$refs.mytodotasks.selected[0] !== undefined) {
        var selectedTask = this.$refs.mytodotasks.selected[0]
        this.setSelectedTask(selectedTask)
        this.$router.push('/leave/edit')
      } else {
        kmsgbox.warn(this.gmxGetTitle('Index.onTaskEdit.warn'))
      }
    },
    onDiagramShow () {
      if (this.$refs.mytodotasks !== undefined &&
        this.$refs.mytodotasks.selected[0] !== undefined) {
        var selectedTask = this.$refs.mytodotasks.selected[0]
        var appInstanceID = selectedTask.AppInstanceID
        var processGUID = selectedTask.ProcessGUID
        window.open('/sfd2/Diagram?AppInstanceID=' + appInstanceID +
          '&ProcessGUID=' + processGUID + '&Mode=' + 'READONLY')
      } else {
        kmsgbox.warn(this.gmxGetTitle('Index.onDiagramShow.warn'))
      }
    },
    onReject () {
      if (this.$refs.mytodotasks !== undefined &&
        this.$refs.mytodotasks.selected[0] !== undefined) {
        var selectedTask = this.$refs.mytodotasks.selected[0]
        this.setSelectedTask(selectedTask)

        var self = this
        var objStore = {
          user: self.loginUser(),
          task: selectedTask,
          callback: function () {
            kmsgbox.info(self.gmxGetTitle('Index.onReject.ok'))
            self.$router.push('/')
          }
        }
        this.rejectTask(objStore)
      } else {
        kmsgbox.warn(this.gmxGetTitle('Index.onReject.warn'))
      }
    }
  }
}
</script>
