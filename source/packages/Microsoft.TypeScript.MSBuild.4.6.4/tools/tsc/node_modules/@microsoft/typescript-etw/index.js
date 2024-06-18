// Attempt to load & return the native module if supported on this platform.
// There will be no exports if the module cannot be loaded

// Only runs on Windows
var loaded = false;
if (process && process.platform === "win32") {
    // Check Node.js requirements. The N-API usage in the module requires Node.js 8.9 or later
    if (process.versions && process.versions.node) {
        var nodeVersionParts = process.versions.node.split('.');
        if (parseInt(nodeVersionParts[0]) === 8 && parseInt(nodeVersionParts[1]) >= 9 || parseInt(nodeVersionParts[0]) >= 10) {
            try {
                module.exports = require(`./bin/${process.arch}/typescript-etw.node`);
                loaded = true;
            } catch (e) {
                // The 'typescript-etw' binary was not able to load
                // It did not build properly or it was built for the wrong architecture
            }
        }
    }
};
// Return no module whatsoever if the native module didn't get loaded
if (!loaded) module.exports = undefined;
