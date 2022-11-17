import html from 'template';
import { ref } from 'vue';

export default {
  template: html` <el-tree-select v-model="value" :data="data" />`,
  setup() {
    const value = ref('');

    const data = [
      {
        value: '1',
        label: 'Level one 1',
        children: [
          {
            value: '1-1',
            label: 'Level two 1-1',
            children: [
              {
                value: '1-1-1',
                label: 'Level three 1-1-1',
              },
            ],
          },
        ],
      },
      {
        value: '2',
        label: 'Level one 2',
        children: [
          {
            value: '2-1',
            label: 'Level two 2-1',
            children: [
              {
                value: '2-1-1',
                label: 'Level three 2-1-1',
              },
            ],
          },
          {
            value: '2-2',
            label: 'Level two 2-2',
            children: [
              {
                value: '2-2-1',
                label: 'Level three 2-2-1',
              },
            ],
          },
        ],
      },
      {
        value: '3',
        label: 'Level one 3',
        children: [
          {
            value: '3-1',
            label: 'Level two 3-1',
            children: [
              {
                value: '3-1-1',
                label: 'Level three 3-1-1',
              },
            ],
          },
          {
            value: '3-2',
            label: 'Level two 3-2',
            children: [
              {
                value: '3-2-1',
                label: 'Level three 3-2-1',
              },
            ],
          },
        ],
      },
    ];
    return {
      value,
      data,
    };
  },
};