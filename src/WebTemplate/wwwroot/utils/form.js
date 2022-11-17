export function getRules(schema) {
  let rules = {};
  for (let key in schema.properties) {
    let property = schema.properties[key];
    if (property.rules) {
      rules[key] = property.rules;
      for (let rule of rules[key]) {
        if (!rule.trigger) {
          rule.trigger = 'blur';
        }
        if (rule.required) {
          rule.message = `请输入${property.title}`;
        } else if (rule.pattern) {
          rule.message = `请输入正确的格式`;
        } else if (rule.email) {
          rule.message = `请输入正确的邮箱格式`;
        } else if (rule.len) {
          rule.message = `${property.title}长度必须是${rule.len}`;
        } else if (rule.regexp) {
          rule.message = `${property.title}格式不正确`;
        } else if (rule.min || rule.max) {
          if (rule.min && rule.max) {
            rule.message = `${property.title}长度需要在${rule.min}到${rule.max}之间`;
          } else if (rule.min) {
            rule.message = `${property.title}长度需要小于${rule.min}`;
          } else if (rule.max) {
            rule.message = `${property.title}长度需要大于${rule.max}`;
          }
        }
      }
    }
  }
  return rules;
}
