import { Injectable } from '@angular/core';

@Injectable()
export class LoggerService {

  loggerMethods: Map<LoggerLevel, (methodName: string, message: string, ...optionalParams: any[]) => any>
    = new Map([
      [LoggerLevel.Debug, this.debug],
      [LoggerLevel.Info, this.info],
      [LoggerLevel.Warn, this.warn],
      [LoggerLevel.Error, this.error],
      [LoggerLevel.Fatal, this.fatal],
    ]);

  log(level: LoggerLevel, methodName: string, message: string, ...optionalParams: any[]) {
    this.loggerMethods.get(level)!(methodName, message, ...optionalParams);
  }

  debug(methodName: string, message: string, ...optionalParams: any[]) {
    console.log(`debug: ${methodName}`);
    console.log(`\t${message}`);
    for (let param of optionalParams) {
      console.log(param);
    }
  }

  info(methodName: string, message: string, ...optionalParams: any[]) {
    console.log(`info: ${methodName}`);
    console.log(`\t${message}`);
    for (let param of optionalParams) {
      console.log(param);
    }
  }

  warn(methodName: string, message: string, ...optionalParams: any[]) {
    console.warn(`warn: ${methodName}`);
    console.warn(`\t${message}`);
    for (let param of optionalParams) {
      console.log(param);
    }
  }

  error(methodName: string, message: string, ...optionalParams: any[]) {
    console.error(`error: ${methodName}`);
    console.error(`\t${message}`);
    for (let param of optionalParams) {
      console.log(param);
    }
  }

  fatal(methodName: string, message: string, ...optionalParams: any[]) {
    console.error(`fatal: ${methodName}`);
    console.error(`\t${message}`);
    for (let param of optionalParams) {
      console.log(param);
    }
  }
}

export enum LoggerLevel {
  All = 0,
  Debug = 1,
  Info = 2,
  Warn = 3,
  Error = 4,
  Fatal = 5,
  Off = 6
}
