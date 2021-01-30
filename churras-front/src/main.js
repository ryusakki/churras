import Vue from 'vue';
import App from './App.vue';
import VueRouter from 'vue-router';

import Login from './views/Login.vue';
import Agenda from './views/Agenda.vue';

import { BootstrapVue, IconsPlugin, BSpinner } from 'bootstrap-vue';

import 'bootstrap/dist/css/bootstrap.css';
import 'bootstrap-vue/dist/bootstrap-vue.css';
import "@/assets/global.css";

Vue.use(BootstrapVue);
Vue.use(IconsPlugin);
Vue.use(VueRouter);
Vue.component('b-spinner', BSpinner);

Vue.config.productionTip = false;

const routes = [
  { path: '/', component: Login },
  { path: '/agenda', component: Agenda }
];

const router = new VueRouter({ routes, mode: 'history' });

new Vue({ router, render: h => h(App) }).$mount('#app');