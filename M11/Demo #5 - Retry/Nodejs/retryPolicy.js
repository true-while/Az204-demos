const sleep = require('system-sleep');

class RetryPolicy {
    constructor(config) {
        this._currentTries = 0;
        this.delay = config.delay;
        this.number_of_retries = config.number_of_retries;
    }

    checkRetries(status) {
        this._currentTries = this._currentTries + 1;
        console.log('Retrying: ' + this._currentTries);

        // Use a delay if this isn't the first try
        if (this._currentTries !== 1) {
            sleep(this.delay);
        }

        if (this._currentTries < this.number_of_retries
            && (status === 500 || status === 504 || status === 403)) {
            return true;
        } else {
            return false;
        }

    }
}
exports.RetryPolicy = RetryPolicy;

