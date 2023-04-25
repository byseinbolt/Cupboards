using System.Collections.Generic;
using System.Linq;
using CupBoardsLevelSettings;
using DG.Tweening;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private GraphView _graphView;
    
    [SerializeField]
    private CupboardsView _cupboardsView;
    
    [SerializeField]
    private PathfindingEngine _pathfindingEngine;
    
    [SerializeField]
    private GraphView _resultGraphView;
    
    [SerializeField]
    private CupboardsView _resultCupboardsView;

    [SerializeField]
    private MovesCountView _movesCountView;

    private Cupboard _selectedCupboard;
    private bool _isMoving;

    private void Start()
    {
        // парсим конфиг уровня
        var levelSettings = new LevelSettings("Assets/LevelSettings/Level1.txt");

        // создаем игровую доску
        _graphView.Initialize(levelSettings);
        // создаем игровые фишки, которые располагаются на графе
        _cupboardsView.Initialize(_graphView, levelSettings.CupboardsStartPositions);
        
        // при клике на ноду - вывываем OnNodeClicked
        // там обрабатываем перемещение выделенной фишки в ноду
        _graphView.OnNodeClicked += OnNodeClicked;
        // при клике на фишку - вывываем OnNodeClicked
        // там обрабатываем выделение фишки и подсветку нод куда можно переместиться
        _cupboardsView.OnCupboardClicked += OnCupboardClicked;
        
        // создаем доску которая должна получится в результате
        _resultGraphView.Initialize(levelSettings);
        _resultCupboardsView.Initialize(_resultGraphView, levelSettings.CupboardsFinishPositions);
    }
    
    private void OnDestroy()
    {
        // Обязательно отписываемся от всех событий на которые мы подписались
        _graphView.OnNodeClicked -= OnNodeClicked;
        _cupboardsView.OnCupboardClicked -= OnCupboardClicked;
    }
    
    private void OnCupboardClicked(Cupboard cupboard)
    {
        // если сейчас идет анимация перемещения фишки - ничего не делаем
        if (_isMoving)
        {
            return;
        }

        // если какая-то фишка уже выделена - сбрасываем эффект
        if (_selectedCupboard != null)
        {
            StopBounceEffectForCupboardAndReachableNodes();
        }

        // если это была та же самая фишка, то больше нам делать ничего не надо, просто выходим 
        // из этого метода
        if (_selectedCupboard == cupboard)
        {
            // сбрасываем выделенную фишку
            _selectedCupboard = null;
            return;
        }
        
        // если все же фишка другая - тогда выделяем ее и пути куда она может пройти
        SelectCupboardAndAllReachableNodes(cupboard);
    }
    
    private void SelectCupboardAndAllReachableNodes(Cupboard cupboard)
    {
        // запоминаем что мы эту фишку выделили 
        _selectedCupboard = cupboard;
        
        // находим все вершины куда мы можем добраться
        foreach (var node in _pathfindingEngine.GetAllReachableNodes(cupboard.Node))
        {
            // проигрываем пульсирующий эффект
            node.PlayBounceAnimation();
        }
    }

    /// <summary>
    /// Метод, который вызывается при клике на ноду.
    /// Если до этого была выделена фишка, то перемещаем ее в ноду (если она достижима)
    /// </summary>
    public void OnNodeClicked(Node clickedNode)
    {
        // если фишка не выделена или фишка уже в движении
        if (_selectedCupboard == null || _isMoving)
        {
            // то просто ничего не делаем
            return;
        }
        
        // иначе: если фишка выделена и она не двигается
        // выключаем эффекты подсветки достижимых нод
        StopBounceEffectForCupboardAndReachableNodes();

        // проверяем - есть ли путь от ноды фишки до ноды по которой мы кликнули
        var path = _pathfindingEngine.GetPath(_selectedCupboard.Node, clickedNode);
        if (path == null)
        {
            // если нету - сбрасываем фишку выделенную и выходим 
            _selectedCupboard = null;
            return;
        }

        // если путь есть - начинаем движение
        StartMovingCupboard(path);
    }

    /// <summary>
    /// Метод для перемещения фишки по пути.
    /// 
    /// Тут мы создаем ряд анимаций для перемещения от ноды к ноде,
    /// и запускаем их.
    /// </summary>
    private void StartMovingCupboard(List<Node> path)
    {
        // отмечаем флаг что движение уже началось
        _isMoving = true;
        
        // создаем последовательность анимаций, которые будут выполняться 
        // одна за одной
        var sequence = DG.Tweening.DOTween.Sequence();
        foreach (var node in path)
        {
            // каждая анимация это пермещение от текущей позиции до позици точки в пути
            sequence.Append(_selectedCupboard.transform.DOMove(node.Position, 0.5f));
        }

        // когда все анимации выполнились сбрасываем флаги (и перестраховываемся - выставляем конечную позицию)
        sequence.OnComplete(() => OnCupboardReachedDestination(path.Last()));
        
        // запускаем последовательность анимаций
        sequence.Play();
    }

    /// <summary>
    /// Выключаем эффекты подсветки достижимых вершин и подсветки нашей фишки
    /// </summary>
    private void StopBounceEffectForCupboardAndReachableNodes()
    {
        var reachableNodes = _pathfindingEngine.GetAllReachableNodes(_selectedCupboard.Node);
        foreach (var reachableNode in reachableNodes)
        {
            reachableNode.StopBounceAnimation();
        }
    }

    /// <summary>
    /// Метод который вызывается когда фишка дошла до точки назначения
    /// </summary>
    private void OnCupboardReachedDestination(Node destination)
    {
        _selectedCupboard.SetPosition(destination);
        _isMoving = false;
        _selectedCupboard = null;
        _movesCountView.ShowCurrentMovesCount();
    }
}