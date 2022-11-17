import html from 'template';
import { ref } from 'vue';

export default {
  template: html`<ul v-infinite-scroll="load" class="infinite-list" style="overflow: auto">
    <li v-for="i in count" :key="i" class="infinite-list-item">{{ i }}</li>
  </ul>`,
  styles: html`<style>
    .infinite-list {
      height: 300px;
      padding: 0;
      margin: 0;
      list-style: none;
    }
    .infinite-list .infinite-list-item {
      display: flex;
      align-items: center;
      justify-content: center;
      height: 50px;
      margin: 10px;
    }
    .infinite-list .infinite-list-item + .list-item {
      margin-top: 10px;
    }
  </style>`,
  setup() {
    const count = ref(10);
    const load = () => {
      count.value += 2;
    };
    return {
      count,
      load,
    };
  },
};
