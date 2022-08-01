import { MessageApiModel } from './message-api.model';

export interface MessagingApiModel {
  user1Name: string;
  user2Name: string;
  messagesList: MessageApiModel[];
}
