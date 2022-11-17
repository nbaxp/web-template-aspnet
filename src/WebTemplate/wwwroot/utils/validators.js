const validators = {
  phone() {
    return function (rule, value, callback) {
      if (value && !/^[\+\-\d]{6,15}$/i.test(value)) {
        return callback(new Error(rule.message))
      }
      return callback();
    }
  },
  remote(model) {
    return function (rule, value, callback) {
      //if (value && !/^(http|https|ftp):\/\/[^\s]+$/i.test(value)) {
      //  return callback(new Error(rule.message))
      //}
      return callback();
    }
  },
  accept() {
    return function (rule, value, callback) {
      var param = (rule.extensions || '.jpe?g,.png,.gif').replaceAll(',', '|');
      if (value && !new RegExp("\\.(" + param + ")$", "i").test(value)) {
        return callback(new Error(rule.message))
      }
      return callback();
    }
  },
  compare(model) {
    return function (rule, value, callback) {
      if (value && value !== model[rule.compare]) {
        return callback(new Error(rule.message))
      }
      return callback();
    }
  },
  creditcard() {
    return function (rule, value, callback) {
      if (/[^0-9 \-]+/.test(value)) {
        return callback(new Error(rule.message))
      }
      var nCheck = 0,
        nDigit = 0,
        bEven = false,
        n, cDigit;

      value = value.replace(/\D/g, "");
      if (value.length < 13 || value.length > 19) {
        return false;
      }

      for (n = value.length - 1; n >= 0; n--) {
        cDigit = value.charAt(n);
        nDigit = parseInt(cDigit, 10);
        if (bEven) {
          if ((nDigit *= 2) > 9) {
            nDigit -= 9;
          }
        }

        nCheck += nDigit;
        bEven = !bEven;
      }
      if ((nCheck % 10) !== 0) {
        return callback(new Error(message));
      }
      return callback();
    }
  }
};

function getRules(schema) {
  let rules = {};
  for (let key in schema.properties) {
    const prop = schema.properties[key];
    if (prop.rules) {
      let propRules = prop.rules;
      propRules.map(o => {
        if (o.validator) {
          o.validator = (validators[o.validator])(data);
        }
        return o;
      });
      rules[key] = prop.rules;
    }
  }
  return rules;
};

export { validators, getRules }