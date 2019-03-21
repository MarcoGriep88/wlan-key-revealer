// This file is required by the index.html file and will
// be executed in the renderer process for that window.
// All of the Node.js APIs are available in this process.

window.$ = window.jQuery = require('jquery');
const { exec } = require('child_process');
$(function() {
    exec('winkey.exe', (error, stdout, stderr) => {
        if (error) {
          console.error(`exec error: ${error}`);
          return;
        }
        document.getElementById('output').innerText= stdout;
        $('#bodyRender').css("background-image", "url()");  
        console.log(`stderr: ${stderr}`);
      });
});
