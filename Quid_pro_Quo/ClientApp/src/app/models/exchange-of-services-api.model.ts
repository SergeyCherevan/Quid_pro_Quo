export interface ExchangeOfServicesApiModel {
  id: number;
  requestingPostId: number;
  requestedPostId: number;
  dateNumberOfRequestedPost: number;
  dateNumberOfRequestingPost: number;

  text: string;
  time: Date;

  proposalStatus: StatusEnum;
  doneStatus1: StatusEnum;
  doneStatus2: StatusEnum;
  notViewed: boolean;
}

export enum StatusEnum {
  noInfo = 0,
  yes = 1,
  no = 2,
}
