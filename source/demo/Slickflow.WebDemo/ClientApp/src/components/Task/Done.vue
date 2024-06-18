<template>
  <q-page>
    <q-table title=""
             :data="taskdata"
             :columns="columns"
             row-key="TaskID">
    </q-table>
    <q-btn :label="ui.refresh" icon="refresh" @click="onLoadTasks" color="primary" style="margin-right:10px;" />
  </q-page>
</template>

<script>
import { mapActions, mapGetters } from 'vuex'

export default {
  name: 'ToDo',
  created () {
    this.onLoadTasks()
  },
  data () {
    return {
      ui: {
        refresh: this.gmxGetTitle('refresh')
      },
      columns: [
        {
          name: 'taskID',
          label: 'ID',
          align: 'left',
          field: 'TaskID'
        },
        {
          name: 'appName',
          label: this.gmxGetTitle('AppName'),
          align: 'left',
          field: 'AppName'
        },
        {
          name: 'appInstanceCode',
          label: this.gmxGetTitle('AppInstanceCode'),
          align: 'left',
          field: 'AppInstanceCode'
        },
        {
          name: 'activityName',
          align: 'left',
          label: this.gmxGetTitle('ActivityName'),
          field: 'ActivityName'
        },
        {
          name: 'createdDateTime',
          label: this.gmxGetTitle('CreatedDateTime'),
          field: 'CreatedDateTime',
          sortable: true
        },
        {
          name: 'activityState',
          label: this.gmxGetTitle('ActivityState'),
          field: 'ActivityState',
          sortable: true
        },
        {
          name: 'taskState',
          label: this.gmxGetTitle('TaskState'),
          field: 'TaskState',
          sortable: true
        },
        {
          name: 'assignedToUserName',
          label: this.gmxGetTitle('AssignedToUserName'),
          field: 'AssignedToUserName',
          sortable: true
        },
        {
          name: 'endedDateTime',
          label: this.gmxGetTitle('EndedDateTime'),
          field: 'EndedDateTime',
          sortable: true
        },
        {
          name: 'endedByUserName',
          label: this.gmxGetTitle('EndedByUserName'),
          field: 'EndedByUserName',
          sortable: true
        }
      ],
      taskdata: [],
      selected: []
    }
  },
  methods: {
    ...mapActions('task', ['getDone']),
    ...mapGetters('task', ['donetasks']),
    ...mapGetters('account', ['loginUser']),
    onLoadTasks () {
      var self = this
      var objStore = {
        user: this.loginUser(),
        callback: function () {
          self.taskdata = self.donetasks()
        }
      }
      this.getDone(objStore)
    }
  }
}
</script>
