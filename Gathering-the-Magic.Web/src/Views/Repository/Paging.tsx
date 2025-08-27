namespace Views.Repository
{
    export function Paging()
    {
        return <div class="paging"></div>;
    }

    export function initializePaging(pagingContainer: HTMLElement, cards: Data.API.Card[])
    {
        pagingContainer["currentPage"] = 1;
        pagingContainer["pageSize"] = 50;
        pagingContainer["items"] = cards;
    }

    export function showPage(pagingContainer: HTMLElement, newPage: number)
    {
        const pages: Data.API.Card[][] = pagingContainer["pages"];
        const list = pagingContainer.closest(".list") as HTMLElement;
        const itemsContainer = list.querySelector(".items");
        pagingContainer["currentPage"] = newPage;

        itemsContainer.clearChildren();
        const pageSize = pagingContainer["pageSize"] as number;
        const items = pagingContainer["items"] as Data.API.Card[];

        const start = (newPage - 1) * pageSize;
        const end = start + pageSize;
        for (let i = start; i < end; ++i)
            if (items[i]) itemsContainer.appendChild(CardTile(items[i]));
        list.scroll({ top: 0 });

        createPageLinks(pagingContainer);
    }

    export function createPageLinks(pagingContainer: HTMLElement)
    {
        const items: Data.API.Card[] = pagingContainer["items"];
        const pageSize: number = pagingContainer["pageSize"];
        const pages = Math.ceil(items.length / pageSize);
        const currentPage: number = pagingContainer["currentPage"];
        const lastPage = pages;

        pagingContainer.clearChildren();
        if (lastPage <= 1) return;

        if (currentPage > 1) pagingContainer.append(<a class="page" onclick={ () => showPage(pagingContainer, currentPage - 1) }>{ "<" }</a>);
        if (currentPage - 2 > 1) pagingContainer.append(<a class="page" onclick={ () => showPage(pagingContainer, 1) }>{ "1" }</a>);
        for (let p = currentPage - 2; p <= currentPage + 2; ++p)
        {
            const page = p;
            if (page < 1) continue;
            if (page > lastPage) continue;
            pagingContainer.append(<a class={ ["page", page == currentPage ? "selected" : null] } onclick={ () => showPage(pagingContainer, page) }>{ page }</a>);
        }
        if (currentPage + 2 < lastPage) pagingContainer.append(<a class="page" onclick={ () => showPage(pagingContainer, lastPage) }>{ lastPage }</a>);
        if (currentPage < lastPage) pagingContainer.append(<a class="page" onclick={ () => showPage(pagingContainer, currentPage + 1) }>{ ">" }</a>);
    }
}