// format html`...` by vscode lit-html
export default function (strings, ...values) {
  let output = '';
  let index;
  for (index = 0; index < values.length; index++) {
    output += strings[index] + values[index];
  }
  output += strings[index];
  return output;
}
