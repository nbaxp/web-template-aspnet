module.exports = {
  root: true,
  env: {
    node: true,
    browser: true,
    es6: true,
  },
  parser: 'vue-eslint-parser',
  parserOptions: {
    ecmaVersion: 2022,
    sourceType: 'module',
  },
  extends: [
    'eslint:recommended', //
    'plugin:vue/vue3-recommended',
    'plugin:prettier/recommended',
  ],
  plugins: [
    'import', //
    'simple-import-sort',
  ],
  rules: {
    'vue/comment-directive': 'off',
    'no-unused-vars': 'warn',
    'import/extensions': ['error', 'ignorePackages', { js: 'always' }],
    'simple-import-sort/imports': 'warn',
    'simple-import-sort/exports': 'warn',
    'import/first': 'error',
    'import/newline-after-import': 'error',
  },
};
