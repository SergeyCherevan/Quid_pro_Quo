import { UserApiModel } from './user-api.model'

export interface UsersPageApiModel {
  users: UserApiModel[];
  usersCount: number;
}
