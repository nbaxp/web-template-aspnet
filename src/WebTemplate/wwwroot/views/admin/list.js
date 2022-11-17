import html from 'template';
import { reactive, ref } from 'vue';

import DataTable from '../../components/table.js';

const template = html`<data-table :schema="schema" :data="data" />`;

export default {
  template,
  components: { DataTable },
  setup() {
    const schema = reactive({
      properties: {
        date: {
          title: '日期',
        },
        name: {
          title: '性名',
        },
        state: {
          title: '状态',
        },
        city: {
          title: '城市',
        },
        address: {
          title: '地址',
        },
        zip: {
          title: '邮编',
        },
      },
    });
    const data = ref([
      {
        date: '2016-05-01',
        name: 'Tom',
        state: 'California',
        city: 'Los Angeles',
        address: 'No. 189, Grove St, Los Angeles',
        zip: 'CA 90036',
      },
      {
        date: '2016-05-02',
        name: 'Tom',
        state: 'California',
        city: 'Los Angeles',
        address: 'No. 189, Grove St, Los Angeles',
        zip: 'CA 90036',
      },
    ]);
    return {
      schema,
      data,
    };
  },
};
