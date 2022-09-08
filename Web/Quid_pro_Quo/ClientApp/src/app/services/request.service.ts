import { Injectable } from "@angular/core";

@Injectable()
export class RequestService {

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
      });
  }
}
