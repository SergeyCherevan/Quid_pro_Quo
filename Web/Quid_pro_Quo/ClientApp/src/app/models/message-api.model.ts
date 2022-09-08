export interface MessageApiModel {
  id: number;
  authorName: string;
  text: string;
  imageFileName: string;
  fileName: string;
  postedAt: Date;
  notViewed: boolean;
}
