import Mock from 'mockjs';
import mockFetch from 'mockjs-fetch';

mockFetch(Mock);
Mock.setup({
  timeout: '200-400',
  debug: true,
});
export default Mock;
