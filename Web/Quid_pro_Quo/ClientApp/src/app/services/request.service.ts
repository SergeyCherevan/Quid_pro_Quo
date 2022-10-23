import { Injectable } from "@angular/core";

import { LoggerService } from "./logger.service";

@Injectable()
export class RequestService {

  constructor(public loggerService: LoggerService) { }

  get(address: string, jwtString?: string) {

    return fetch(address, {
      method: 'GET',
      headers: {
        'Content-Type': 'application/json;charset=utf-8',
        'Authorization': `Bearer ${jwtString}`,
      },
    })
      .then(async response => {
        if (response.ok) {
          try {
            return response.json();
          } catch {
            return {};
          }
        } else {
          throw Error((await response.json()).error);
        }
      })
      .then(response => {
        this.loggerService.info('RequestService.get()', `HTTP GET request by address: ${address};
\tJWT string: '${jwtString}'`);
        this.loggerService.info('RequestService.get()', `response:`, response);

        return response;
      })
      .catch((error: Error) => {
        this.loggerService.info('RequestService.get()', `HTTP GET request by address: ${address};
\tJWT string: '${jwtString}'`);
        this.loggerService.error('RequestService.get()', `Error ${error.name},
\tmessage: ${error.message}
\tby stack: ${error.stack}`);

        throw error;
      });
  }

  post(address: string, postObj: object, jwtString?: string) {

    return fetch(address, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json;charset=utf-8',
        'Authorization': `Bearer ${jwtString}`,
      },
      body: JSON.stringify(postObj),
    })
      .then(async response => {
        if (response.ok) {
          try {
            return response.json();
          } catch {
            return {};
          }
        } else {
          throw Error((await response.json()).error);
        }
      })
      .then(response => {
        this.loggerService.info('RequestService.post()', `HTTP POST request by address: ${address};
\tJWT string: '${jwtString}'
\trequest body:`, postObj);
        this.loggerService.info('RequestService.post()', `response:`, response);

        return response;
      })
      .catch((error: Error) => {
        this.loggerService.info('RequestService.post()', `HTTP POST request by address: ${address};
\tJWT string: '${jwtString}'
\trequest body:`, postObj);
        this.loggerService.error('RequestService.post()', `Error ${error.name},
\tmessage: ${error.message}
\tby stack: ${error.stack}`);

        throw error;
      });
  }

  postMultipartForm(address: string, formData: FormData, jwtString?: string) {

    return fetch(address, {
      method: 'POST',
      headers: {
        'Authorization': `Bearer ${jwtString}`,
      },
      body: formData,
    })
      .then(async response => {
        if (response.ok) {
          try {
            return response.json();
          } catch {
            return {};
          }
        } else {
          throw Error((await response.json()).error);
        }
      })
      .then(response => {
        this.loggerService.info('RequestService.postMultipartForm()', `HTTP POST request by address: ${address};
\tJWT string: '${jwtString}'
\trequest body:`, formData);
        this.loggerService.info('RequestService.postMultipartForm()', `response:`, response);

        return response;
      })
      .catch((error: Error) => {
        this.loggerService.info('RequestService.postMultipartForm()', `HTTP POST request by address: ${address};
\tJWT string: '${jwtString}'
\trequest body:`, formData);
        this.loggerService.error('RequestService.postMultipartForm()', `Error ${error.name},
\tmessage: ${error.message}
\tby stack: ${error.stack}`);

        throw error;
      });
  }

  put(address: string, postObj: object, jwtString?: string) {

    return fetch(address, {
      method: 'PUT',
      headers: {
        'Content-Type': 'application/json;charset=utf-8',
        'Authorization': `Bearer ${jwtString}`,
      },
      body: JSON.stringify(postObj),
    })
      .then(async response => {
        if (response.ok) {
          try {
            return response.json();
          } catch {
            return { };
          }
        } else {
          throw Error((await response.json()).error);
        }
      })
      .then(response => {
        this.loggerService.info('RequestService.put()', `HTTP PUT request by address: ${address};
\tJWT string: '${jwtString}'
\trequest body:`, postObj);
        this.loggerService.info('RequestService.put()', `response:`, response);

        return response;
      })
      .catch((error: Error) => {
        this.loggerService.info('RequestService.put()', `HTTP PUT request by address: ${address};
\tJWT string: '${jwtString}'
\trequest body:`, postObj);
        this.loggerService.error('RequestService.put()', `Error ${error.name},
\tmessage: ${error.message}
\tby stack: ${error.stack}`);

        throw error;
      });
  }

  putMultipartForm(address: string, formData: FormData, jwtString?: string) {

    return fetch(address, {
      method: 'PUT',
      headers: {
        'Authorization': `Bearer ${jwtString}`,
      },
      body: formData,
    })
      .then(async response => {
        if (response.ok) {
          try {
            return response.json();
          } catch {
            return {};
          }
        } else {
          throw Error((await response.json()).error);
        }
      })
      .then(response => {
        this.loggerService.info('RequestService.putMultipartForm()', `HTTP PUT request by address: ${address};
\tJWT string: '${jwtString}'
\trequest body:`, formData);
        this.loggerService.info('RequestService.get()', `response:`, response);

        return response;
      })
      .catch((error: Error) => {
        this.loggerService.info('RequestService.get()', `HTTP GET request by address: ${address};
\tJWT string: '${jwtString}'
\trequest body:`, formData);
        this.loggerService.error('RequestService.get()', `Error ${error.name},
\tmessage: ${error.message}
\tby stack: ${error.stack}`);

        throw error;
      });
  }

  delete(address: string, postObj?: object, jwtString?: string) {

    return fetch(address, {
      method: 'DELETE',
      headers: {
        'Content-Type': 'application/json;charset=utf-8',
        'Authorization': `Bearer ${jwtString}`,
      },
      body: postObj ? JSON.stringify(postObj) : undefined,
    })
      .then(async response => {
        if (response.ok) {
          try {
            return response.json();
          } catch {
            return {};
          }
        } else {
          throw Error((await response.json()).error);
        }
      })
      .then(response => {
        this.loggerService.info('RequestService.delete()', `HTTP DELETE request by address: ${address};
\tJWT string: '${jwtString}'
\trequest body:`, postObj);
        this.loggerService.info('RequestService.get()', `response:`, response);

        return response;
      })
      .catch((error: Error) => {
        this.loggerService.info('RequestService.get()', `HTTP GET request by address: ${address};
\tJWT string: '${jwtString}'
\trequest body:`, postObj);
        this.loggerService.error('RequestService.get()', `Error ${error.name},
\tmessage: ${error.message}
\tby stack: ${error.stack}`);

        throw error;
      });
  }
}
