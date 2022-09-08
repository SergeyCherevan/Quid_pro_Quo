import { Injectable } from "@angular/core";

@Injectable()
export class DictionaryService {

  dictionary: Map<string, string> = new Map();

  constructor() { }

  get(key: string): string {
    return this.dictionary.get(key) ?? key;
  }
}
