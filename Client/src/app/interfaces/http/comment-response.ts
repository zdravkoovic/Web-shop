import { Comments } from "../comments";

export interface CommentResponse {
    comments: Comments[];
    rating: number;
    comment: Comments;
}
