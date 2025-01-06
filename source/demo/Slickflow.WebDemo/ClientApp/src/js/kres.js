const kres = (function () {
  function kres () {

  }

  kres.init = function (en, zh) {
    var lang = kres.getLang()
    if (lang === 'en') {
      kres.mxCurrentLanguageJSON = en
    } else if (lang === 'zh') {
      kres.mxCurrentLanguageJSON = zh
    }
  }

  kres.mxCurrentLanguageJSON = null
  kres.MX_RESOURCE_LOCAL_LANGUAGE = 'mx-resource-local-lang'

  kres.getLang = function () {
    // get user enviroment language
    var localLang = window.localStorage.getItem(kres.MX_RESOURCE_LOCAL_LANGUAGE)
    if (localLang === null || localLang === '') {
      var isIE = navigator.userAgent.indexOf('MSIE') >= 0
      var lan = (isIE) ? navigator.userLanguage : navigator.language

      if (lan.substring(0, 2) !== 'zh') {
        window.localStorage.setItem(kres.MX_RESOURCE_LOCAL_LANGUAGE, 'en')
      } else {
        window.localStorage.setItem(kres.MX_RESOURCE_LOCAL_LANGUAGE, 'zh')
      }
      localLang = window.localStorage.getItem(kres.MX_RESOURCE_LOCAL_LANGUAGE)
    }
    return localLang
  }

  kres.setLang = function (lang) {
    window.localStorage.setItem(kres.MX_RESOURCE_LOCAL_LANGUAGE, lang)
  }

  kres.get = function (key) {
    var json = kres.mxCurrentLanguageJSON
    var text = json[key]
    return text
  }
  return kres
})()

export default kres
