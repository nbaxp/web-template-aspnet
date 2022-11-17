import {
  __commonJS
} from "__commonJS";

// node_modules/mockjs-fetch/index.js
var require_mockjs_fetch = __commonJS({
  "node_modules/mockjs-fetch/index.js"(exports, module) {
    function mockFetch(Mock) {
      if (!Mock || !Mock.mock) {
        throw new Error("Mock.js is required.");
      }
      const tempFetchName = "__mockFetchRawFetch__";
      if (window[tempFetchName]) {
        return;
      }
      window[tempFetchName] = window.fetch;
      window.fetch = function(url, options) {
        options = options || { method: "GET" };
        const method = options.method;
        if (Mock.XHR._settings.debug) {
          console.log(`${method} ${url}`, "options: ", options);
        }
        for (const key in Mock._mocked) {
          const item = Mock._mocked[key];
          const urlMatch = typeof item.rurl === "string" && url.indexOf(item.rurl) >= 0 || item.rurl instanceof RegExp && item.rurl.test(url);
          const methodMatch = !item.rtype || item.rtype === method;
          if (urlMatch && methodMatch) {
            let timeout = Mock.XHR._settings.timeout || "200-400";
            if (typeof timeout === "string") {
              const temp = timeout.split("-").map((item2) => parseInt(item2));
              timeout = temp[0] + Math.round(Math.random() * (temp[1] - temp[0]));
            }
            options.url = url;
            return new Promise((resolve) => {
              const resp = typeof item.template === "function" ? item.template.call(this, options) : Mock.mock(item.template);
              setTimeout(() => {
                resolve({
                  status: 200,
                  text() {
                    return Promise.resolve(JSON.stringify(resp));
                  },
                  json() {
                    return Promise.resolve(resp);
                  },
                  blob() {
                    return Promise.resolve(resp);
                  },
                  formData() {
                    return Promise.resolve(resp);
                  },
                  arrayBuffer() {
                    return Promise.resolve(resp);
                  }
                });
                if (Mock.XHR._settings.debug) {
                  console.log("resp: ", resp);
                }
              }, timeout);
            });
          }
        }
        return window[tempFetchName](url, options);
      };
    }
    module.exports = mockFetch;
  }
});

// dep:mockjs-fetch
var mockjs_fetch_default = require_mockjs_fetch();
export {
  mockjs_fetch_default as default
};
