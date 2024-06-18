<template>
  <div class="q-pa-md" style="max-width: 400px">

    <q-form @submit="onSubmit"
            @reset="onReset"
            class="q-gutter-md">
      <h4>{{ui.leaveinfo}}</h4>
      <q-select filled
                v-model="leave.LeaveType"
                :options="leaveTypeOptions"
                :label="ui.leavetype" />

      <q-input filled
               type="number"
               v-model.number="leave.Days"
               :label="ui.days"
               lazy-rules
               :rules="[ val=> val !== null && val !== '' || 'Please type leave days', val => val > 0 && val < 100 || 'Please type a valid days' ]" />

      <q-input ref="date"
               filled
               typeof="date"
               v-model="leave.FromDate"
               :label="ui.fromdate"
               lazy-rules
               :rules="[ val => val !== null && val !== '' || 'Please select a date']"
               @click="$refs.qDateProxy.show()">
        <template v-slot:append>
          <q-icon name="event" class="cursor-pointer">
            <q-popup-proxy ref="qDateProxy">
              <q-date type="date" v-model="leave.FromDate"
                      @input="$refs.qDateProxy.hide()" />
            </q-popup-proxy>
          </q-icon>
        </template>
      </q-input>

      <q-input filled
               type="textarea"
               v-model="leave.Remark"
               :label="ui.remark" />
      <div>
        <q-btn :label="ui.submit" icon="save_alt" type="submit" color="primary" />
        <q-btn :label="ui.reset" icon="undo" type="reset" color="green" />
        <q-btn :label="ui.return" icon="keyboard_return" color="orange" to="/" />
      </div>
    </q-form>

  </div>
</template>

<script>
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
        submit: this.gmxGetTitle('submit'),
        reset: this.gmxGetTitle('reset'),
        return: this.gmxGetTitle('return')
      },
      leave: { LeaveType: '', Days: 0, FromDate: '', Remark: '', Opinions: '' },
      leaveTypeOptions: [
        this.gmxGetTitle('Personal'),
        this.gmxGetTitle('Vocation'),
        this.gmxGetTitle('Sickness')
      ]
    }
  },
  methods: {
    ...mapActions('leave', ['submitLeave']),
    ...mapGetters('account', ['loginUser']),
    onSubmit () {
      var self = this
      var payload = {
        entity: this.leave,
        user: this.loginUser(),
        callback: function () {
          kmsgbox.info(self.gmxGetTitle('Leave.Add.submitLeave.ok'))
        }
      }
      this.submitLeave(payload)
      this.$root.$refs.todocomponent.onLoadTasks()
      this.$router.push('/')
    },
    onReset () {
      this.leave.Days = 0
      this.leave.Remark = ''
      this.leave.FromDate = null
    }
  }
}
</script>
