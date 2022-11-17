import Mock from './mock.js';

//fetch('/api/user/login',{method:'post',headers: {"Content-Type": "application/json"}}).then(o=>o.text())
export default function () {
  Mock.mock(new RegExp('/api/user/login'), 'post', (o) => {
    console.log(o);
    return { token: 1 };
  });
}
