<template>
  <q-page class="q-pa-md">
    <div class="loginPage fixed-center">

      <h4>
        <!--<q-avatar>
    <img src="https://cdn.quasar.dev/img/avatar.png">
  </q-avatar>-->
        <q-avatar icon="person" color="orange" text-color="white"></q-avatar>
        {{ui.login}}
      </h4>
      <q-form @submit="onSaveLoginUser">
        <q-select filled
                  type="list" v-model="selectedRole" :options="roleList" option-value="ID" option-label="RoleName"
                  @input="onRoleChanged" @focus="onRoleFocused"
                  style="width:340px;" :label=ui.selectrole
                  lazy-rules
                  :rules="[ val=> val !== null && val !== '' || 'Please select a role']"></q-select>
        <q-select filled
                  type="list" v-model="selectedUser" :options="userList" option-value="UserID" option-label="UserName"
                  @focus="onUserFocused"
                  style="width:340px;" :label=ui.selectuser
                  lazy-rules
                  :rules="[ val=> val !== null && val !== '' || 'Please select a user']"></q-select>
        <br />
        <div class="row">
          <q-space />
          <q-btn :label="ui.login" type="submit" color="primary" style="margin-right:10px;"/>
          <q-btn :label="ui.cancel" type="reset" color="primary" to="/" flat class="q-ml-sm" />
        </div>
      </q-form>
    </div>
  </q-page>
</template>

<script>
import axios from 'axios'
import { mapActions, mapGetters } from 'vuex'

export default {
  name: 'Login',
  data () {
    return {
      ui: {
        login: this.gmxGetTitle('login'),
        cancel: this.gmxGetTitle('cancel'),
        selectrole: this.gmxGetTitle('selectrole'),
        selectuser: this.gmxGetTitle('selectuser')
      },
      roleList: null,
      userList: null,
      selectedRole: null,
      selectedUser: null
    }
  },
  created () {
    this.getRoles()
  },
  methods: {
    ...mapActions('ru', ['getRoles', 'getUsers']),
    ...mapGetters('ru', ['roles', 'users']),
    ...mapActions('account', ['saveAccount']),
    showMessage () {
      this.$q.dialog({
        title: 'Alert',
        message: 'Some message'
      }).onOk(() => {
        // console.log('OK')
      }).onCancel(() => {
        // console.log('Cancel')
      }).onDismiss(() => {
        // console.log('I am triggered on both OK and Cancel')
      })
    },
    onRoleFocused () {
      this.roleList = this.roles()
    },
    onRoleChanged (item) {
      var roleID = item.ID
      this.getUsers(roleID)
    },
    onUserFocused () {
      this.userList = this.users()
    },
    onSaveLoginUser () {
      var account = {
        Role: this.selectedRole,
        User: this.selectedUser
      }
      this.saveAccount(account)
      window.location.href = ''
    }
  }
}
</script>

<style scoped>
  .loginPage {
    border: 1px solid #CCCCCC;
    background-color: #FFFFFF;
    margin: auto;
    padding: 20px;
  }
</style>
