import { PostGetApiModel } from './post-get-api.model'

export interface PostsPageApiModel {
  posts: PostGetApiModel[];
  postsCount: number;
}
