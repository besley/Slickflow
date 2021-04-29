<template>
  <div class="q-pa-md" style="max-width: 400px">
    <q-form @submit="onSubmit"
            class="q-gutter-md">
      <h4>{{ui.leaveinfo}}</h4>
      <q-select filled
                v-model="leaveInfo.LeaveType"
                :options="leaveTypeOptions"
                :label="ui.leavetype"
                readonly/>

      <q-input filled
               type="number"
               v-model.number="leaveInfo.Days"
               :label="ui.days"
               readonly
               lazy-rules
               :rules="[ val=> val !== null && val !== '' || 'Please type leave days', val => val > 0 && val < 100 || 'Please type a valid days' ]" />

      <q-input ref="date"
               filled
               typeof="date"
               v-model="leaveInfo.FromDate"
               :label="ui.fromdate"
               readonly
               lazy-rules
               :rules="[ val => val !== null && val !== '' || 'Please select a date']"
               @click="$refs.qDateProxy.show()">
        <template v-slot:append>
          <q-icon name="event" class="cursor-pointer">
            <q-popup-proxy ref="qDateProxy">
              <q-date type="date" v-model="leaveInfo.FromDate" readonly
                      @input="$refs.qDateProxy.hide()" />
            </q-popup-proxy>
          </q-icon>
        </template>
      </q-input>

      <q-input filled
               type="textarea"
               v-model="leaveInfo.Remark"
               :label="ui.remark"
               readonly/>
      <q-input filled
               type="textarea"
               v-model="leaveInfo.Opinions"
               :label="ui.opinions" />

      <div>
        <q-btn :label="ui.save" icon="playlist_add_check" type="submit" color="primary" />
        <q-btn :label="ui.nextstep" icon="forward_to_inbox"  color="green" @click="showStepDialog"></q-btn>
        <q-btn :label="ui.return" icon="keyboard_return" color="orange" to="/" />
      </div>
    </q-form>

  </div>
</template>

<script>
import NextStepTree from 'components/Step/NextStepTree'
import { mapActions, mapGetters } from 'vuex'
import kmsgbox from '../../js/kmsgbox.js'

export default {
  name: 'Leave',
  data () {
    return {
      ui: {
        leaveinfo: this.gmxGetTitle('leaveinfo'),
        leavetype: this.gmxGetTitle('LeaveType'),
        days: this.gmxGetTitle('Days'),
        fromdate: this.gmxGetTitle('FromDate'),
        remark: this.gmxGetTitle('Remark'),
        opinions: this.gmxGetTitle('Opinions'),
        save: this.gmxGetTitle('save'),
        nextstep: this.gmxGetTitle('nextstep'),
        forward: this.gmxGetTitle('forward'),
        submit: this.gmxGetTitle('submit'),
        reset: this.gmxGetTitle('reset'),
        return: this.gmxGetTitle('return')
      },
      leaveInfo: { ID: 0, LeaveType: '', Days: 0, FromDate: '', Remark: '', Opinions: '' }, // get from store
      leaveTypeOptions: [
        this.gmxGetTitle('Personal'),
        this.gmxGetTitle('Vocation'),
        this.gmxGetTitle('Sickness')
      ]
    }
  },
  created () {
    var task = this.selectedtask()
    var appInstanceID = task.AppInstanceID

    var self = this
    var objStore = {
      leaveID: appInstanceID,
      callback: function () {
        var currentLeave = self.currentLeave()
        self.leaveInfo.ID = currentLeave.ID
        self.leaveInfo.LeaveType = currentLeave.LeaveType
        self.leaveInfo.Days = currentLeave.Days
        self.leaveInfo.FromDate = currentLeave.FromDate
        self.leaveInfo.Remark = currentLeave.Remark
        self.leaveInfo.Opinions = currentLeave.Opinions
      }
    }
    this.getLeave(objStore)
  },
  methods: {
    ...mapActions('leave', ['getLeave', 'saveLeave']),
    ...mapGetters('leave', ['currentLeave']),
    ...mapGetters('task', ['selectedtask']),
    ...mapGetters('account', ['loginUser']),
    onSubmit () {
      var self = this
      var payload = {
        Leave: this.leaveInfo,
        User: this.loginUser,
        callback: function (result) {
          if (result.Status === 1) {
            kmsgbox.info(self.gmxGetTitle('Leave.Edit.onSubmit.ok'))
          } else {
            kmsgbox.error(self.gmxGetTitle('Leave.Edit.onSubmit.error'), result.Message)
          }
          self.$router.push('')
        }
      }
      this.saveLeave(payload)
    },
    showStepDialog () {
      var todo = this.selectedtask()
      if (todo === undefined || todo.length === 0) {
        kmsgbox.warn(this.gmxGetTitle('Leave.Edit.showStepDialog.warn'))
        return
      }

      var conditions = {
        days: this.leaveInfo.Days.toString()
      }

      this.$q.dialog({
        component: NextStepTree,

        // optional if you want to have access to
        // Router, Vuex store, and so on, in your
        // custom component:
        parent: this, // becomes child of this Vue node
        // ("this" points to your Vue component)
        // (prop was called "root" in < 1.1.0 and
        // still works, but recommending to switch
        // to the more appropriate "parent" name)

        // props forwarded to component
        // (everything except "component" and "parent" props above):
        text: 'NextStepTree',
        task: todo,
        conditions: conditions
        // ...more.props...
      }).onOk(() => {
        // console.log('OK')
        this.$router.push('/')
      }).onCancel(() => {
        // console.log('Cancel')
      }).onDismiss(() => {
        // console.log('Called on OK or Cancel')
      })
    }
  }
}
</script>
