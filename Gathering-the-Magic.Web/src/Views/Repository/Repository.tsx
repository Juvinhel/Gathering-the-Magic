namespace Views.Repository
{
    export function Repository()
    {
        return <div class="repository">
            <Search.SearchElement />
            <List.CardListElement />
        </div>;
    }
}