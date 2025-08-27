namespace Views.Repository
{
    export function List()
    {
        let currentSearch: AsyncIterablePromise<Data.API.Card>;
        async function showItems(list: HTMLElement, search: AsyncIterablePromise<Data.API.Card>)
        {
            const repository = list.closest(".repository") as HTMLElement;
            const itemsContainer = repository.querySelector(".items") as HTMLElement;
            const pagingContainer = repository.querySelector(".paging") as HTMLElement;
            const editor = repository.closest(".editor") as HTMLElement;
            const footer = editor.querySelector(".footer");
            const cardCount = footer.querySelector(".card-count") as HTMLSpanElement;
            const searchRunning = footer.querySelector(".search-running");
            cardCount.textContent = "0";
            itemsContainer.clearChildren();
            pagingContainer.clearChildren();

            currentSearch = search;
            if (!search) return;

            searchRunning.classList.remove("none");
            try
            {
                const cards: Data.API.Card[] = [];
                initializePaging(pagingContainer, cards);
                for await (const card of search)
                {
                    if (currentSearch != search) return;
                    cards.push(card);
                    cardCount.textContent = cards.length.toFixed();
                    if (cards.length <= 50)
                        itemsContainer.appendChild(CardTile(card));
                    else if (cards.length % 50 == 0)
                        createPageLinks(pagingContainer);
                }
                createPageLinks(pagingContainer);
            }
            catch (error)
            {
                UI.Dialog.error(error);
            }
            searchRunning.classList.add("none");
        }

        return <div class="list"
            showItems={ function (this: HTMLElement, search: AsyncIterablePromise<Data.API.Card>) { showItems(this, search); } }>
            <div class="items" onchildrenchanged={ childrenChanged } />
            <Paging />
        </div>;
    }

    function childrenChanged(event: UI.Events.ChildrenChangedEvent)
    {
        const target = event.currentTarget as HTMLElement;
        const editor = target.closest(".editor");
        const workbench = editor.querySelector("my-workbench") as Views.Workbench.WorkbenchElement;
        const deck = workbench.getData();
        const entries = Data.getEntries(deck);

        for (const child of event.addedNodes)
            if (child instanceof HTMLElement)
            {
                const card: Data.API.Card = child["card"];
                const isInDeck = entries.some(e => e.name == card.name);
                child.classList.toggle("is-in-deck", isInDeck);
            }
    }
}