<template>
  <q-layout view="lHh Lpr lFf">
    <q-header elevated>
      <q-toolbar>
        <q-btn flat
               dense
               round
               icon="menu"
               aria-label="Menu"
               @click="leftDrawerOpen = !leftDrawerOpen" />

        <q-toolbar-title>
          Slickflow.WebDemo
        </q-toolbar-title>

        <q-btn-dropdown color="primary" label="Lang" style="padding-right:120px">
          <q-list>
            <q-item clickable v-close-popup @click="onEnglishClick">
              <q-item-section>
                <q-item-label>English</q-item-label>
              </q-item-section>
            </q-item>

            <q-item clickable v-close-popup @click="onChineseClick">
              <q-item-section>
                <q-item-label>Chinese</q-item-label>
              </q-item-section>
            </q-item>
          </q-list>
        </q-btn-dropdown>

        <q-btn to="/login"
               flat
               icon-right="account_circle" :label="ui.login" class="absolute-right"></q-btn>
      </q-toolbar>
    </q-header>

    <q-drawer
      v-model="leftDrawerOpen"
      show-if-above
      bordered
      content-class="bg-grey-1"
    >
      <q-list>
        <q-item-label
          header
          class="text-grey-8"
        >
          Essential Links
        </q-item-label>
        <EssentialLink
          v-for="link in essentialLinks"
          :key="link.title"
          v-bind="link"
        />
      </q-list>
    </q-drawer>

    <q-page-container>
      <router-view />
    </q-page-container>
  </q-layout>
</template>

<script>
import EssentialLink from 'components/EssentialLink.vue'
import kres from '../js/kres.js'

const linksData = [
  {
    title: kres.get('designer'),
    caption: 'designer',
    icon: 'public',
    link: 'http://demo.slickflow.com/sfd/'
  },
  {
    title: kres.get('github'),
    caption: 'slickflow',
    icon: 'code',
    link: 'https://github.com/besley/slickflow'
  }
]

export default {
  name: 'MainLayout',
  components: { EssentialLink },
  data () {
    return {
      ui: {
        login: this.gmxGetTitle('login')
      },
      leftDrawerOpen: false,
      essentialLinks: linksData
    }
  },
  methods: {
    onEnglishClick () {
      kres.setLang('en')
      window.location.href = ''
    },
    onChineseClick () {
      kres.setLang('zh')
      window.location.href = ''
    }
  }
}
</script>
