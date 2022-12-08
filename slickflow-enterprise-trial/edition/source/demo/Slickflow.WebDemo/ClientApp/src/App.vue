<template>
  <div id="q-app">
    <router-view />
  </div>
</template>
<script>
import Vue from 'vue'
import zh from './static/resources/zh.json'
import en from './static/resources/en.json'
import kres from './js/kres.js'

Vue.mixin({
  methods: {
    gmxGetTitle: function (key) {
      var title = kres.get(key)
      if (title === null) { title = key }
      return title
    }
  }
})

export default {
  name: 'App',

  created () {
    kres.init(this.myJsonEN, this.myJsonZH)
  },
  data () {
    return {
      myJsonZH: zh,
      myJsonEN: en
    }
  }
}
</script>
