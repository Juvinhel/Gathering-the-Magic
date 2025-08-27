namespace Views.Editor
{
    export function Editor()
    {
        let selectedCard: Data.API.Card;
        let hoveredCard: Data.API.Card;
        async function cardSelected(event: CustomEvent) 
        {
            const editor = event.currentTarget as HTMLElement;
            const cardElement = event.target as HTMLElement;
            selectedCard = event.detail.card as Data.API.Card;

            for (const element of editor.querySelectorAll(".card-container.selected"))
                if (element != cardElement)
                    element.classList.remove("selected");

            const cardInfo = editor.querySelector(".card-info");
            cardInfo["loadData"](hoveredCard ?? selectedCard);
        }

        async function cardHovered(event: CustomEvent) 
        {
            const editor = event.currentTarget as HTMLElement;
            hoveredCard = event.detail.card as Data.API.Card;

            const cardInfo = editor.querySelector(".card-info");
            cardInfo["loadData"](hoveredCard ?? selectedCard);
        }

        let oldMode = null;
        function sizeChanged(event: UI.Events.SizeChangedEvent)
        {   // gets called once oninsert anyway
            const editor = event.currentTarget as HTMLElement;

            const isSmall = event.newSize.width < 1024;
            if (oldMode !== isSmall)
            {
                oldMode = isSmall;
                const repository = editor.querySelector(".repository") as HTMLElement;
                const cardInfo = editor.querySelector(".card-info") as HTMLElement;
                const workbench = editor.querySelector("my-workbench") as Workbench.WorkbenchElement;
                repository.remove();
                cardInfo.remove();
                workbench.remove();

                for (const child of [...editor.children])
                {
                    if (child.classList.contains("menu")) continue;
                    if (child.classList.contains("footer")) continue;
                    child.remove();
                }

                if (isSmall)
                    editor.appendChild(<swipe-container class="container" index={ 1 }>{ repository }{ workbench }{ cardInfo }</swipe-container>);
                else
                    editor.appendChild(<pane-container class="container"><div>{ repository }</div><div>{ cardInfo }</div><div>{ workbench }</div></pane-container>);
            }
        }

        function deckChanged(event: Event)
        {
            const target = event.currentTarget as HTMLElement;
            const unsavedProgress = target.querySelector(".unsaved-progress") as HTMLElement;
            unsavedProgress.classList.toggle("none", false);
        }

        return <div class="editor" onsizechanged={ sizeChanged } oncardselected={ cardSelected } oncardhovered={ cardHovered } ondeckchanged={ deckChanged }>
            <Menu />
            <Repository.Repository />
            <Info.CardInfo />
            <Workbench.WorkbenchElement />
            <Footer />
        </div>;
    }
}