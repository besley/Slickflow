<template>
  <q-page>
    <q-table title=""
             :data="myleavedata"
             :columns="columns"
             row-key="ID">
    </q-table>
  </q-page>
</template>

<script>
import { mapActions, mapGetters } from 'vuex'

export default {
  name: 'MyLeave',
  created () {
    this.onLoadLeaves()
  },
  data () {
    return {
      columns: [
        {
          name: 'id',
          label: 'ID',
          align: 'left',
          field: 'ID'
        },
        {
          name: 'leaveType',
          label: this.gmxGetTitle('LeaveType'),
          align: 'left',
          field: 'LeaveType'
        },
        {
          name: 'days',
          label: this.gmxGetTitle('Days'),
          align: 'left',
          field: 'Days'
        },
        {
          name: 'fromDate',
          align: 'left',
          label: this.gmxGetTitle('FromDate'),
          field: 'FromDate'
        },
        {
          name: 'remark',
          label: this.gmxGetTitle('Remark'),
          field: 'Remark',
          sortable: true
        },
        {
          name: 'opinions',
          label: this.gmxGetTitle('Opinions'),
          field: 'Opinions',
          sortable: true
        },
        {
          name: 'createdDateTime',
          label: this.gmxGetTitle('CreatedDateTime'),
          field: 'CreatedDateTime',
          sortable: true
        },
        {
          name: 'CreatedUserName',
          label: this.gmxGetTitle('CreatedUserName'),
          field: 'CreatedUserName',
          sortable: true
        }
      ],
      myleavedata: []
    }
  },
  methods: {
    ...mapActions('leave', ['queryLeave']),
    ...mapGetters('account', ['loginUser']),
    onLoadLeaves () {
      var self = this
      var objStore = {
        user: this.loginUser(),
        callback: function (leavelist) {
          self.myleavedata = leavelist
        }
      }
      this.queryLeave(objStore)
    }
  }
}
</script>
