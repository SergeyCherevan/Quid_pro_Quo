import { StatusEnum } from './exchange-of-services-api.model';
import { PostGetApiModel } from './post-get-api.model';

export interface ExchangeOfServicesClientModel {
  id: number;
  requestingPostId: number;
  requestedPostId: number;
  dateNumberOfRequestingPost: number;
  dateNumberOfRequestedPost: number;

  text: string;
  time: Date;

  proposalStatus: StatusEnum;
  doneStatus1: StatusEnum;
  doneStatus2: StatusEnum;
  notViewed: boolean;

  requestingPost: PostGetApiModel;
  requestedPost: PostGetApiModel;
}
