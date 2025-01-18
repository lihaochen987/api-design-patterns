"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var header_1 = require("@repo/ui/header");
require("./style.css");
var typescript_svg_1 = require("/typescript.svg");
var counter_1 = require("@repo/ui/counter");
var setup_counter_1 = require("@repo/ui/setup-counter");
document.querySelector("#app").innerHTML = "\n  <div>\n    <a href=\"https://vitejs.dev\" target=\"_blank\">\n      <img src=\"/vite.svg\" class=\"logo\" alt=\"Vite logo\" />\n    </a>\n    <a href=\"https://www.typescriptlang.org/\" target=\"_blank\">\n      <img src=\"".concat(typescript_svg_1.default, "\" class=\"logo vanilla\" alt=\"TypeScript logo\" />\n    </a>\n    ").concat((0, header_1.Header)({ title: "Docs" }), "\n    <div class=\"card\">\n      ").concat((0, counter_1.Counter)(), "\n    </div>\n  </div>\n");
(0, setup_counter_1.setupCounter)(document.querySelector("#counter"));
