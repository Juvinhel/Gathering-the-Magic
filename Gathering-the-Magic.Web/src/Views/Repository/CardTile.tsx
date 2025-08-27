namespace Views.Repository
{
    export function CardTile(card: Data.API.Card)
    {
        return <div class={ ["card-container", "card-tile"] } card={ card } onrightclick={ (event: PointerEvent) => showContextMenu(event, card) } onclick={ select } ondblclick={ addCard } onmouseenter={ enter } onmouseleave={ leave } draggable={ true } ondragstart={ dragStart }>
            <div class="image"><img src="img/card-back.png" lazy-image={ card.img } /></div>
            <color-icon class="in-deck" src="img/icons/deck.svg" />
        </div>;
    }

    const clickables = ["INPUT", "A", "BUTTON"];
    function select(event: MouseEvent)
    {
        if (event.composedPath().some(x => clickables.includes((x as HTMLElement).tagName))) return;

        const cardTile = event.currentTarget as HTMLElement;

        const card = cardTile["card"] as Data.API.Card;
        const result = cardTile.classList.toggle("selected");
        const selectedEvent = new CustomEvent("cardselected", { bubbles: true, detail: { card: result ? card : null } });
        cardTile.dispatchEvent(selectedEvent);
    }

    function enter(event: MouseEvent)
    {
        const cardTile = event.currentTarget as HTMLElement;
        const card = cardTile["card"] as Data.API.Card;
        const selectedEvent = new CustomEvent("cardhovered", { bubbles: true, detail: { card } });
        cardTile.dispatchEvent(selectedEvent);
    }

    function leave(event: MouseEvent)
    {
        const cardTile = event.currentTarget as HTMLElement;
        const selectedEvent = new CustomEvent("cardhovered", { bubbles: true, detail: { card: null } });
        cardTile.dispatchEvent(selectedEvent);
    }

    function addCard(event: MouseEvent)
    {
        const cardTile = event.currentTarget as HTMLElement;
        const card: Data.API.Card = cardTile["card"];
        const editor = cardTile.closest(".editor") as HTMLElement;
        const workbench = editor.querySelector("my-workbench") as Workbench.WorkbenchElement;
        if (workbench) workbench.addCards(card);
    }

    function dragStart(event: DragEvent)
    {
        event.stopPropagation();
        const target = event.currentTarget as HTMLElement;
        const card: Data.Entry = target["card"];

        event.dataTransfer.setData("text", JSON.stringify(card));
        event.dataTransfer.effectAllowed = "all";
    }

    function showContextMenu(event: PointerEvent, card: Data.API.Card)
    {
        UI.ContextMenu.show(event,
            <menu-button title="Insert Card" onclick={ (event: Event) => insertCard(event, card) }><color-icon src="img/icons/arrow-right.svg" /><span>Insert into ...</span></menu-button>,
            <menu-button title="Scryfall" onclick={ () => window.open(card.links.Scryfall, '_blank') }><color-icon src="img/icons/scryfall-black.svg" /><span>Scryfall</span></menu-button>,
            card.links.EDHREC ? <menu-button title="EDHREC" onclick={ () => window.open(card.links.EDHREC, '_blank') }><color-icon src="img/icons/edhrec.svg" /><span>EDHREC</span></menu-button> : null,
        );
    }

    async function insertCard(event: Event, card: Data.API.Card)
    {
        const workbench = document.querySelector("my-workbench") as Workbench.WorkbenchElement;
        const sectionElements = [...workbench.querySelectorAll("my-section") as NodeListOf<Workbench.SectionElement>];
        const sectionTitles = sectionElements.map(x => Views.Workbench.getSectionPath(x));

        const result = await Workbench.selectSection(sectionTitles);
        if (!result) return;
        const selectedSection = sectionElements[result.index];

        const entry: Data.Entry = Object.clone(card) as Data.Entry;
        entry.quantity = 1;
        const list = selectedSection.querySelector(".list");
        const entryElement = new Workbench.EntryElement(entry);
        list.append(entryElement);
        entryElement.scrollIntoView({ behavior: "smooth" });
    }
}