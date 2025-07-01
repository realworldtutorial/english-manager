export default defineEventHandler(() => {
    // This function handles the event when a user requests the posts.
    // It returns a list of posts.
    const posts = [
        {
            id: 1,
            title: 'First Post',
            content: 'This is the content of the first post.',
            slug: 'first-post'
        },
        {
            id: 2,
            title: 'Second Post',
            content: 'This is the content of the second post.',
            slug: 'second-post'
        }
    ];
    return posts;
})