import html from 'template';

const template = html`<h2>test</h2>`;

const styles = html` <style>
  h1 {
    color: green;
  }
</style>`;

const Test2 = {
  template: html`<h1>Test2</h1>`,
  styles,
};

export default {
  template,
  styles,
};

export { Test2 };